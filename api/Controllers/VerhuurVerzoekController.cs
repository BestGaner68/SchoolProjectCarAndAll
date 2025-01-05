using System.Security.Claims;
using api.Dtos.Account;
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
        private readonly IVoertuigHelper _voertuigHelper;
        public VerhuurVerzoekController(IVerhuurVerzoekService verhuurVerzoekRepo, IVoertuigHelper voertuighelper)
        {
            _verhuurVerzoekRepo = verhuurVerzoekRepo;
            _voertuigHelper = voertuighelper;
        }

        [HttpGet("GetAllPendingVerhuurVerzoeken")]
        public async Task<IActionResult> GetAllPendingVerhuurVerzoeken()
        {
            var Verzoeken = await _verhuurVerzoekRepo.GetAllAsync();
            var filtered = Verzoeken.Where(x => x.Status == "Pending");
            return Ok(filtered.Select(x => x.ToVerhuurVerzoekDto())); 
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
            if (!await _voertuigHelper.CheckDatesAsync(verhuurVerzoekDto.VoertuigId, verhuurVerzoekDto.StartDatum, verhuurVerzoekDto.EindDatum))
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
            var dates = await _voertuigHelper.GetUnavailableDates(voertuigId);
            return Ok(dates);
        }
        
        [HttpGet("GetVoertuigStatus/{voertuigId}")]
        public async Task<IActionResult> GetVoertuigStatus([FromRoute] int voertuigId)
        {
            var status = await _voertuigHelper.CheckStatusAsync(voertuigId);
            return Ok(status);
        }
         
    }
}