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
        
        [HttpGet("AllAutos")]
        public async Task<IActionResult> GetAllAutos(){
            return Ok(await _voertuigService.GetAllAutos());
        }
    }
}