using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Interfaces;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/FrontOfficeMedewerker")]
    public class FrontOfficeMedewerkerController : ControllerBase
    {
        private readonly IReserveringService _reserveringService;
        private readonly IVoertuigService _voertuigService;
        private readonly IKostenService _kostenService;
        private readonly IFactuurService _factuurService;
        public FrontOfficeMedewerkerController(IReserveringService reserveringService, IVoertuigService VoertuigService, IKostenService kostenService, IFactuurService factuurService){
            _reserveringService = reserveringService;
            _voertuigService = VoertuigService;
            _kostenService = kostenService;
            _factuurService = factuurService;
        }

        [HttpGet("GetAllVoertuigData/{voertuigId}")]
        public async Task<IActionResult> GetAllVoertuigData ([FromRoute]int voertuigId)
        {
            try
            {
                var voertuigData = await _voertuigService.GetAllDataVoertuig(voertuigId);
                return Ok(voertuigData);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
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
        public async Task<IActionResult> NeemIn([FromBody] InnameDto innameDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
        
                if (innameDto.IsSchade)
                {
                    var currentresult = await _reserveringService.MeldSchadeVanuitReservering(innameDto.ReserveringId, innameDto.Schade, innameDto.BeschrijvingFoto);
                    if (!currentresult)
                    {
                        return BadRequest("Er is iets mis gegaan bij het melden van de schade.");
                    }
                }
                else
                {
                    var result = await _reserveringService.NeemIn(innameDto.ReserveringId);
                    if (!result)
                    {
                        return BadRequest("Er is iets mis gegaan bij het afronden van de reservering.");
                    }
                }
        
                var reservering = await _reserveringService.GetReserveringById(innameDto.ReserveringId);
                if (reservering == null)
                {
                    return BadRequest("Reservering niet gevonden.");
                }
        
                var prijsOverzicht = await _kostenService.BerekenDaadWerkelijkPrijs(innameDto.ReserveringId, innameDto.GeredenKilometers, innameDto.IsSchade, reservering.AppUserId);
                if (prijsOverzicht == null)
                {
                    return BadRequest("Er is iets mis gegaan bij het berekenen van de prijs.");
                }
        
                var AppUserId = reservering.AppUserId;
                var factuur = await _factuurService.MaakFactuur(reservering, prijsOverzicht, AppUserId);
                var emailResult = await _factuurService.StuurFactuurPerEmail(factuur);
                if (!emailResult)
                {
                    return BadRequest("Er is een fout opgetreden bij het verzenden van de factuur.");
                }
        
                return Ok("Reservering is compleet en factuur is verzonden.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
        
                return StatusCode(500, "Er is een onverwachte fout opgetreden.");
            }
        }          
    }
}