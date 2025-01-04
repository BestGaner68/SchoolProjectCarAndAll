using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ReserveringenEnSchade;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/UitgifteVoertuigen")]
    public class FrontdeskMedewerkerCarUitgifteController : ControllerBase
    {
        private readonly IReserveringService _reserveringService;
        public FrontdeskMedewerkerCarUitgifteController(IReserveringService reserveringService){
            _reserveringService = reserveringService;
        }

        [HttpPut("GeefUit")]
        public async Task<IActionResult> GeefUit (int ReserveringId){
            var result = await _reserveringService.GeefUit(ReserveringId);
            if (!result)
            {
                return BadRequest("Er is iets mis gegaan");
            }
            return Ok("Reservering is uitgegeven");
        }

        [HttpPut("NeemIn")]
        public async Task<IActionResult> NeemIn (int ReserveringId, InnameDto innameDto){
            if (innameDto.isSchade)
            {
                if (!ModelState.IsValid){
                    return BadRequest("Graag beschrijven wat de voertuigschade betreft.");
                }
                var currentresult = await _reserveringService.MeldSchade(ReserveringId, innameDto.Schade);
                if (!currentresult)    
                {
                    return BadRequest("Er is iets mis gegaan");
                }
                return Ok("Schade succesvol gemeld");
            }
            var result = await _reserveringService.NeemIn(ReserveringId);
            if (!result)
            {
                return BadRequest("Er is iets mis gegaan");
            }
            return Ok("Reservering is compleet");
        }
    
    }
}