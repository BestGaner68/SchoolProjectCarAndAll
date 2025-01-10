using System.Security.Claims;
using api.Dtos.Account;
using api.Dtos.Verhuur;
using api.Interfaces;
using api.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/verhuurVerzoek")]
    public class VerhuurVerzoekController : ControllerBase
    {
        private readonly IVerhuurVerzoekService _verhuurVerzoekRepo;
        private readonly IVoertuigService _voertuigService;
        public VerhuurVerzoekController(IVerhuurVerzoekService verhuurVerzoekRepo, IVoertuigService voertuigService)
        {
            _verhuurVerzoekRepo = verhuurVerzoekRepo;
            _voertuigService = voertuigService;
        }

        [HttpGet("GetAllPendingVerhuurVerzoeken")]
        public async Task<IActionResult> GetAllPendingVerhuurVerzoeken()
        {
            var pendingVerzoeken = await _verhuurVerzoekRepo.GetPendingAsync();

            if (!pendingVerzoeken.Any())
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

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var verhuurVerzoekByID = await _verhuurVerzoekRepo.GetByIdAsync(id);
            if (verhuurVerzoekByID == null)
            {
                return NotFound();
            }
            return Ok(verhuurVerzoekByID);
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
            if (!await _voertuigService.CheckDatesAsync(verhuurVerzoekDto.VoertuigId, verhuurVerzoekDto.StartDatum, verhuurVerzoekDto.EindDatum))
            {
                return BadRequest("Aangegeven data zijn al in gebruik, het voertuig kan niet worden verhuurd");
            }
            var verhuurVerzoekModel = verhuurVerzoekDto.ToVerhuurVerzoekFromDto(userId);
            await _verhuurVerzoekRepo.CreateAsync(verhuurVerzoekModel);
            return CreatedAtAction(nameof(GetById), new {id = verhuurVerzoekModel.VerhuurVerzoekId}, verhuurVerzoekModel.ToVerhuurVerzoekDto());
        }

        [HttpGet("GetUnavailableData/{voertuigId}")]
        public async Task<IActionResult> GetUnavailableData([FromRoute] int voertuigId)
        {
            var dates = await _voertuigService.GetUnavailableDates(voertuigId);
            return Ok(dates);
        }
        
        [HttpGet("GetVoertuigStatus/{voertuigId}")]
        public async Task<IActionResult> GetVoertuigStatus([FromRoute] int voertuigId)
        {
            var status = await _voertuigService.CheckStatusAsync(voertuigId);
            return Ok(status);
        }

        [HttpGet("GetMyVerzoeken")]
        public async Task<IActionResult> GetMyVerzoeken(){
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserVerzoeken = await _verhuurVerzoekRepo.GetMyVerhuurVerzoeken(AppUserId);
            if (!UserVerzoeken.Any()){
                return NotFound( new {message = "Er zijn geen verhuurverzoeken gevonden."});
            }
            return Ok (UserVerzoeken);
        }

        [HttpPut("DeclineMyVerzoek/{VerhuurVerzoekId}")]
        public async Task<IActionResult> DeclineMyVerzoek ([FromRoute]int VerhuurVerzoekId){
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Succes = await _verhuurVerzoekRepo.DeclineMyVerzoek(VerhuurVerzoekId, AppUserId);
            if (Succes)
            {
                return Ok();
            }
            return BadRequest(new { message = "De operatie kon niet worden uitgevoerd." });
        } 
    }
}