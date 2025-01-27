using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Interfaces;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/FrontOfficeMedewerker")]
    [ApiController]
    public class FrontOfficeMedewerkerController : ControllerBase
    {
        private readonly IReserveringService _reserveringService;
        private readonly IVoertuigService _voertuigService;
        private readonly IKostenService _kostenService;
        private readonly IFactuurService _factuurService;
    
        public FrontOfficeMedewerkerController(IReserveringService reserveringService, IVoertuigService voertuigService, IKostenService kostenService, IFactuurService factuurService)
        {
            _reserveringService = reserveringService;
            _voertuigService = voertuigService;
            _kostenService = kostenService;
            _factuurService = factuurService;
        }
    
        [HttpGet("GetAllVoertuigData/{voertuigId}")] //methode voor opvragen van alle voertuigdata
        [Authorize (Roles = Rollen.BackendWorker)]
        public async Task<IActionResult> GetAllVoertuigData([FromRoute] int voertuigId)
        {
            try
            {
                var voertuigData = await _voertuigService.GetAllDataVoertuig(voertuigId);
                if (voertuigData == null)
                {
                    return NotFound(new { message = "Voertuigdata niet gevonden." });
                }
                return Ok(voertuigData);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    
        [HttpPut("GeefVoertuigUit")] //methode voor het updaten van voertuigstatusen en bijhouden van uitgifte voertuig
        [Authorize (Roles = Rollen.BackendWorker)]
        public async Task<IActionResult> GeefUit([FromBody] IdDto reserveringDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _reserveringService.GeefUit(reserveringDto.Id);
                if (!result)
                {
                    return BadRequest("Er is iets mis gegaan bij het uitgeven van het voertuig.");
                }
                return Ok("Reservering is uitgegeven.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een onverwachte fout opgetreden.", details = ex.Message });
            }
        }
    
        [HttpPut("NeemIn")] //methode voor innemen van voertuigen en sturen van factuur
        [Authorize (Roles = Rollen.BackendWorker)]
        public async Task<IActionResult> NeemIn([FromBody] InnameDto innameDto)
        {       
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Ingevulde informatie is niet correct ingevult", errors });
            }

            try
            {
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
                    return NotFound(new { message = "Reservering niet gevonden." });
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
                return StatusCode(500, new { message = "Er is een onverwachte fout opgetreden.", details = ex.Message });
            }
        }
    }
}