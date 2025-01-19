using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/FrontOfficeMedewerker")]
    public class FrontOfficeMedewerkerController : ControllerBase
    {
        private readonly IReserveringService _reserveringService;
        public FrontOfficeMedewerkerController(IReserveringService reserveringService){
            _reserveringService = reserveringService;
        }

        [HttpPut("GeefVoertuigUit")]
        public async Task<IActionResult> GeefUit ([FromBody]IdDto reserveringDto){
            var result = await _reserveringService.GeefUit(reserveringDto.Id);
            if (result == false)
            {
                return BadRequest("Er is iets mis gegaan");
            }
            return Ok("Reservering is uitgegeven");
        }

        [HttpPut("NeemIn")]
        public async Task<IActionResult> NeemIn ([FromBody]InnameDto innameDto){
            if (!ModelState.IsValid)
            {
                return BadRequest("Vul alle verplichte velden in.");
            }
            
            if (innameDto.IsSchade)
            {
                var currentresult = await _reserveringService.MeldSchadeVanuitReservering(innameDto.ReserveringId, innameDto.Schade, innameDto.BeschrijvingFoto);
                if (!currentresult)    
                {
                    return BadRequest("Er is iets mis gegaan");
                }
                return Ok("Schade succesvol gemeld");
            }
            var result = await _reserveringService.NeemIn(innameDto.ReserveringId);
            if (!result)
            {
                return BadRequest("Er is iets mis gegaan");
            }
            return Ok("Reservering is compleet");
        }
    
    }
}