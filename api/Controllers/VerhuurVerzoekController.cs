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
        public VerhuurVerzoekController(IVerhuurVerzoekService verhuurVerzoekRepo, IVoertuigService voertuigService, IKostenService kostenService, IBetaalService betaalService)
        {
            _verhuurVerzoekRepo = verhuurVerzoekRepo;
            _voertuigService = voertuigService;
            _kostenService = kostenService;
            _betaalService = betaalService;
        }

        [HttpGet("GetAllPendingVerhuurVerzoeken")]
        public async Task<IActionResult> GetAllPendingVerhuurVerzoeken()
        {
            var pendingVerzoeken = await _verhuurVerzoekRepo.GetPendingAsync();

            if (pendingVerzoeken.Count == 0)
            {
                return NotFound(new { message = "Geen openstaande verhuurverzoeken gevonden." });
            }
            return Ok(pendingVerzoeken);
        }

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var verhuurVerzoekByID = await _verhuurVerzoekRepo.GetByIdAsync(id);
            if (verhuurVerzoekByID == null)
            {
                return NotFound();
            }
            return Ok(verhuurVerzoekByID);
        }

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

        [Authorize]
        [HttpPost("VerhuurVerzoekRequest")]
        public async Task<IActionResult> Create([FromBody] VerhuurVerzoekRequestDto verhuurVerzoekDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            if (!await _voertuigService.CheckDatesAsync(verhuurVerzoekDto.VoertuigId, verhuurVerzoekDto.StartDatum, verhuurVerzoekDto.EindDatum))
            {
                return BadRequest(new {message = "Aangegeven data zijn al in gebruik, het voertuig kan dan niet worden verhuurd"});
            }
            if (!await _voertuigService.IsAvailable(verhuurVerzoekDto.VoertuigId))
            {
                return BadRequest(new {message = "Voertuig is momenteel niet in gebruik, controllleer de status of kies een ander voertuig"});
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

        [HttpGet("GetUnavailableData/{voertuigId}")]
        public async Task<IActionResult> GetUnavailableData([FromRoute] int voertuigId)
        {
            var dates = await _voertuigService.GetUnavailableDates(voertuigId);
            return Ok(dates);
        }
        

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

        [HttpPut("GetKostenOverzicht")]
        public async Task<IActionResult> GetKostenOverzicht([FromBody]IdDto VerhuurverzoekId)
        {
            try
            {
                // var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // if (string.IsNullOrEmpty(appUserId))
                //     return Unauthorized();

                var kosten = await _kostenService.BerekenVerwachtePrijsUitVerhuurVerzoek(VerhuurverzoekId.Id);

                return Ok(kosten);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpPost("process-credit-card")]
        public async Task<IActionResult> ProcessCreditCard([FromBody] BetaalDto betaalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Alle velden zijn verplicht.");
            }
            var succes = await _betaalService.BehandelCreditCardGegevens(betaalDto);
            return Ok(succes);
        }

    }
}