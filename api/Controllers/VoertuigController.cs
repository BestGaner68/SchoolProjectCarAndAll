using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/voertuigen")]   
    public class VoertuigController : ControllerBase
    {
        private readonly IVoertuigService _voertuigService;
        public VoertuigController(IVoertuigService voertuigService){
            _voertuigService = voertuigService;
        }
        
        [HttpGet("AllVoertuigen")]
        public async Task<IActionResult> GetAllVoertuigen(){
            return Ok(await _voertuigService.GetAllVoertuigen());
        }

        [HttpGet("GetVoertuigByMerk")]
        public async Task<IActionResult> GetVoertuigenByMerk(string VoertuigMerk){
            var voertuigen = await _voertuigService.GetVoertuigenByMerk(VoertuigMerk);
            if (!voertuigen.Any()) 
            {
                return NotFound(new { Message = $"Geen voertuigen gevonden met merk: {VoertuigMerk}" });
            }
            return Ok(voertuigen);
        }

        [HttpGet("GetVoertuigBySoort")]
        public async Task<IActionResult> GetVoertuigenBySoort(string VoertuigSoort){
            var voertuigen = await _voertuigService.GetVoertuigenBySoort(VoertuigSoort);
            if (!voertuigen.Any()) 
            {
                return NotFound(new { Message = $"Geen voertuigen gevonden met merk: {VoertuigSoort}" });
            }
            return Ok(voertuigen);
        }
        [HttpGet("GetVoertuigByDate")]
        public async Task<IActionResult> GetVoertuigenByDate( DateTime Voertuigstartdate, DateTime Voertuigenddate)
        {
            var voertuigen = await _voertuigService.GetVoertuigenByDate( Voertuigstartdate, Voertuigenddate);
            if (!voertuigen.Any())
            {
                return NotFound(new { Message = $"Geen voertuigen gevonden met datum: {Voertuigstartdate} {Voertuigenddate}" });
            }
            return Ok(voertuigen);
        }
    }
}