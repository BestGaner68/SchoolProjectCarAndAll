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


        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpGet("GetAllPendingVerhuurVerzoeken")] //methode returned verhuurverzoeken die verwerkt moeten worden
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

        [HttpGet("GetByID/{id}")] //verhuurverzoek ophalen gebaseerd op id
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var verhuurVerzoekByID = await _verhuurVerzoekRepo.GetByIdAsync(id);
                if (verhuurVerzoekByID == null)
                {
                    return NotFound(new { message = "Verhuurverzoek niet gevonden." });
                }
                return Ok(verhuurVerzoekByID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van verhuurverzoek.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("VerhuurVerzoekRequest")] //maakt een verhuurverzoek request, gebruik in het huren van voertuigen
        public async Task<IActionResult> Create([FromBody] VerhuurVerzoekRequestDto verhuurVerzoekDto)
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
                    return BadRequest(new { message = $"StartDatum en EindDatum moeten minimaal 24 uur uit elkaar liggen. Huidige verschil: {timeDifference.TotalHours} uren." });
                }

                var verhuurVerzoekModel = verhuurVerzoekDto.ToVerhuurVerzoekFromDto(userId);
                await _verhuurVerzoekRepo.CreateAsync(verhuurVerzoekModel);

                return CreatedAtAction(nameof(GetById), new { id = verhuurVerzoekModel.VerhuurVerzoekId }, verhuurVerzoekModel.ToVerhuurVerzoekDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het aanmaken van verhuurverzoek.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetUnavailableData/{voertuigId}")] //returned data waarop een voertuig niet verhuurd kan worden
        public async Task<IActionResult> GetUnavailableData([FromRoute] int voertuigId)
        {
            try
            {
                var dates = await _voertuigService.GetUnavailableDates(voertuigId);
                return Ok(dates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van onbeschikbare datums.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetMyVerzoeken")] //returned de verhuurverzoeken van een gebruiker
        public async Task<IActionResult> GetMyVerzoeken()
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized(new { message = "JWT Token is niet meer in gebruik" });
                }

                var UserVerzoeken = await _verhuurVerzoekRepo.GetMyVerhuurVerzoeken(AppUserId);
                if (!UserVerzoeken.Any())
                {
                    return NotFound(new { message = "Er zijn geen verhuurverzoeken gevonden." });
                }

                return Ok(UserVerzoeken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van mijn verhuurverzoeken.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("DeclineMyVerzoek/{VerhuurVerzoekId}")] //verwijderd een verzoek van een gebruiker kan altijd gebeuren totdat het is goedgekeurt
        public async Task<IActionResult> DeclineMyVerzoek([FromRoute] int VerhuurVerzoekId)
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized(new { message = "JWT Token is niet meer in gebruik" });
                }

                var Succes = await _verhuurVerzoekRepo.DeclineMyVerzoek(VerhuurVerzoekId, AppUserId);
                if (Succes)
                {
                    return Ok(new { message = "Verhuurverzoek succesvol afgewezen." });
                }

                return BadRequest(new { message = "De operatie kon niet worden uitgevoerd." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het afwijzen van verhuurverzoek.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("GetKostenOverzicht")] //stuur een kostenoverzicht naar de frontend, gebruik bij het tonen van de kosten van het reserveren
        public async Task<IActionResult> GetKostenOverzicht([FromBody] GetKostenOverzichtDto getKostenOverzichtDto)
        {
            try
            {
                var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized(new { message = "JWT Token is niet meer in gebruik" });
                }

                var kosten = await _kostenService.BerekenVerwachtePrijsUitVerhuurVerzoek(
                    appUserId,
                    getKostenOverzichtDto.VerwachtteKM,
                    getKostenOverzichtDto.StartDatum,
                    getKostenOverzichtDto.EindDatum,
                    getKostenOverzichtDto.VoertuigId
                );

                return Ok(kosten);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van kostenoverzicht.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
        
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