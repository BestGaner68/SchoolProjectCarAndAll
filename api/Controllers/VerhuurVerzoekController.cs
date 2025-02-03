using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.Account;
using api.Dtos.Betalingen;
using api.Dtos.Verhuur;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/verhuurVerzoek")]
    public class VerhuurVerzoekController : ControllerBase
    {
        private readonly IVerhuurVerzoekService _verhuurVerzoekRepo;
        private readonly IVoertuigService _voertuigService;
        private readonly IKostenService _kostenService;
        private readonly IBetaalService _betaalService;
        private readonly ILogger<VerhuurVerzoekController> _logger;

        public VerhuurVerzoekController(IVerhuurVerzoekService verhuurVerzoekRepo, IVoertuigService voertuigService, IKostenService kostenService, IBetaalService betaalService, ILogger<VerhuurVerzoekController> logger)
        {
            _verhuurVerzoekRepo = verhuurVerzoekRepo;
            _voertuigService = voertuigService;
            _kostenService = kostenService;
            _betaalService = betaalService;
            _logger = logger;
        }


        /// <summary>
        /// returned alle verhuurverzoeken die status pending hebben dus nog moeten worden verwerkt gebruik in frontend
        /// </summary>
        /// <returns>alle verhuurverzoeken met status pending</returns>
        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpGet("GetAllPendingVerhuurVerzoeken")] 
        public async Task<IActionResult> GetAllPendingVerhuurVerzoeken()
        {
            try
            {
                var pendingVerzoeken = await _verhuurVerzoekRepo.GetPendingAsync();

                if (pendingVerzoeken.Count == 0)
                {
                    return NotFound(new { message = "Geen openstaande verhuurverzoeken gevonden." });
                }

                var mappedData = new List<VolledigeDataDto>();
                foreach (var verzoek in pendingVerzoeken)
                {
                    var volledigeData = await _verhuurVerzoekRepo.GetVolledigeDataDto(verzoek);
                    mappedData.Add(volledigeData);
                }

                return Ok(mappedData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van openstaande verhuurverzoeken.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }

        /// <summary>
        /// voor het ophalen van een verhuurverzoek op id
        /// </summary>
        /// <param name="id">id verhuurverzoek</param>
        /// <returns>een verhuurverzoek</returns>
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var verhuurVerzoekByID = await _verhuurVerzoekRepo.GetByIdAsync(id);
            if (verhuurVerzoekByID == null)
            {
                return NotFound();
            }
            return Ok(verhuurVerzoekByID);
        }

        /// <summary>
        /// returned alle mogelijke verzekering die gebruikers kunnen kiezen bij verhuurverzoek aanmaken. gebruik bij aanmaken van elementen in frontend
        /// </summary>
        /// <returns>alle verzekering uit de db</returns>
        [HttpGet("GetAllVerzekeringen")]
        public async Task<ActionResult<IEnumerable<Verzekering>>> GetAllVerzekeringen()
        {
            var verzekeringen = await _verhuurVerzoekRepo.GetAllVerzekeringen();
            if (verzekeringen == null)
            {
                return NotFound();
            }
            return Ok(verzekeringen);
        }

        /// <summary>
        /// returned alle accessoires die gebruikers kunnen kiezen bij verhuurverzoek aanmaken. gebruik bij aanmaken van elementen in frontend
        /// </summary>
        /// <returns>alle accessoires uit de db</returns>
        [HttpGet("GetAllAccessoires")]
        public async Task<ActionResult<IEnumerable<Accessoires>>> GetAllAccessoires()
        {
            var accessoires = await _verhuurVerzoekRepo.GetAllAccessoires();
            if (accessoires == null)
            {
                return NotFound();
            }
            return Ok(accessoires);
        }
        /// <summary>
        /// gebruiker kan dit gebruiker om een verhuurverzoekrequest aan te maken
        /// </summary>
        /// <param name="verhuurVerzoekDto">alle data bij aanmaken verhuurverzoek, voertuig, data, verwachttekm, etc.</param>
        /// <returns>niets</returns>
        [Authorize]
        [HttpPost("VerhuurVerzoekRequest")] //maakt een verhuurverzoek request, gebruik in het huren van voertuigen
        public async Task<IActionResult> CreateVerzoek([FromBody] VerhuurVerzoekRequestDto verhuurVerzoekDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "JWT Token is niet meer in gebruik" });
                }

                if (!await _voertuigService.CheckDatesAsync(verhuurVerzoekDto.VoertuigId, verhuurVerzoekDto.StartDatum, verhuurVerzoekDto.EindDatum))
                {
                    return BadRequest(new { message = "Aangegeven data zijn al in gebruik, het voertuig kan dan niet worden verhuurd." });
                }

                if (!await _voertuigService.IsAvailable(verhuurVerzoekDto.VoertuigId))
                {
                    return BadRequest(new { message = "Voertuig is momenteel niet beschikbaar, controleer de status of kies een ander voertuig." });
                }

                var timeDifference = verhuurVerzoekDto.EindDatum - verhuurVerzoekDto.StartDatum;

            if (timeDifference.TotalDays < 2)
            {
                return BadRequest($"StartDatum en EindDatum moeten minimaal 24 uur uit elkaar liggen. {timeDifference}");
            }
            var GekozenAccesoires = await _verhuurVerzoekRepo.FromIdToInstanceAccessoires(verhuurVerzoekDto.AccessoiresIds);
            var verzekering = await _verhuurVerzoekRepo.FromIdToInstanceVerzekering(verhuurVerzoekDto.VerzekeringId);
            var verhuurVerzoekModel = verhuurVerzoekDto.ToVerhuurVerzoekFromDto(userId, GekozenAccesoires, verzekering);
            await _verhuurVerzoekRepo.CreateAsync(verhuurVerzoekModel);
            return CreatedAtAction(nameof(GetById), new {id = verhuurVerzoekModel.VerhuurVerzoekId}, verhuurVerzoekModel.ToVerhuurVerzoekDto());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// returned de data van een voertuig waar het niet verhuurd kan worden
        /// </summary>
        /// <param name="voertuigId">het voertuig waarvan je het wil weten</param>
        /// <returns>alle data waarop het voertuig wordt verhuurd</returns>
        [HttpGet("GetUnavailableData/{voertuigId}")]
        public async Task<IActionResult> GetUnavailableData([FromRoute] int voertuigId)
        {
            var dates = await _voertuigService.GetUnavailableDates(voertuigId);
            return Ok(dates);
        }
        
        /// <summary>
        /// returned alle verzoeken die de gebruiker heeft gemaakt, gebaseerd op zijn jwt token
        /// </summary>
        /// <returns>alle verzoeken die een gebruiker heeft gemaakt</returns>
        [HttpGet("GetMyVerzoeken")]
        public async Task<IActionResult> GetMyVerzoeken(){
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(AppUserId))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            var UserVerzoeken = await _verhuurVerzoekRepo.GetMyVerhuurVerzoeken(AppUserId);
            if (!UserVerzoeken.Any()){
                return NotFound( new {message = "Er zijn geen verhuurverzoeken gevonden."});
            }
            return Ok (UserVerzoeken);
        }

        /// <summary>
        /// gebruiker kan methode gebruiken om zijn verhuurverzoek te verwijderen, DIT is voordat het een reservering is geworden. 
        /// </summary>
        /// <param name="VerhuurVerzoekId">id van het verzoek, gebruiker kan vorige methode hiervoor gebruiken</param>
        /// <returns>niets</returns>
        [HttpPut("DeclineMyVerzoek/{VerhuurVerzoekId}")]
        public async Task<IActionResult> DeclineMyVerzoek ([FromRoute]int VerhuurVerzoekId){
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(AppUserId))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            var Succes = await _verhuurVerzoekRepo.DeclineMyVerzoek(VerhuurVerzoekId, AppUserId);
            if (Succes)
            {
                return Ok();
            }
            return BadRequest(new { message = "De operatie kon niet worden uitgevoerd." });
        }

        /// <summary>
        /// Methode maakt een prijs overzicht gebaseerd op de verwachtte prijs van het verhuurverzoek, gebruik in frontend voor helderheid kosten. Deze methode verschilt van de normale prijs calculatie
        /// aangezien we hier een verhuurverzoek gebruiken en niet een reservering
        /// </summary>
        /// <param name="VerhuurverzoekId">id van de reservering</param>
        /// <returns>een overzicht in de vorm een kostenDto hierin staat het item en de calculatie van de prijs</returns>
        [HttpPut("GetKostenOverzicht")]
        public async Task<IActionResult> GetKostenOverzicht([FromBody]IdDto VerhuurverzoekId)
        {
            try
            {
                var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized(new { message = "JWT Token is niet meer in gebruik" });
                }

                var kosten = await _kostenService.BerekenVerwachtePrijsUitVerhuurVerzoek(VerhuurverzoekId.Id);

                return Ok(kosten);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van kostenoverzicht.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
        
        /// <summary>
        /// mock methode voor het betalen met credit card
        /// </summary>
        /// <param name="betaalDto">gegevens bij het betalen creditcard nummer etc. wordt niet opgeslagen</param>
        /// <returns>niets</returns>
        [Authorize]
        [HttpPost("process-credit-card")] //Mock methode is een beginnende implementatie van het verwerken van creditcard gegevens
        public async Task<IActionResult> ProcessCreditCard([FromBody] BetaalDto betaalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Alle velden zijn verplicht.");
            }

            try
            {
                var succes = await _betaalService.BehandelCreditCardGegevens(betaalDto);
                return Ok(new { message = "Betaling succesvol verwerkt." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het verwerken van creditcard.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
    }
}