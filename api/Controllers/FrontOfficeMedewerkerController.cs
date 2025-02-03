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

        /// <summary>
        /// vraagt alle informatie van een specifiek voertuig op gebaseerd op het id
        /// </summary>
        /// <param name="voertuigId">Id van het voertuig</param>
        /// <returns>informatie van het voertuig en informatie over de status merk, kenteken, naam, kilometerprijs, status etc.</returns>
        [HttpGet("GetAllVoertuigData/{voertuigId}")] 
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

        /// <summary>
        /// frontoffice worker kan dit gebruiken bij het uitgeven van een voertuig om de status hiervan te updaten en bij te houden dat het voertuig is opgehaalt
        /// </summary>
        /// <param name="reserveringDto">Id van de reservering</param>
        /// <returns>niets</returns>
        [HttpPut("GeefVoertuigUit")] 
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

        /// <summary>
        /// gebruik bij inname voertuig, de medewerker kan hier invullen hoeveel km er zijn gereden en of er schade was aan het voertuig hierna wordt er een factuur gemaakt en opgestuurd naar de gebruiker
        /// </summary>
        /// <param name="innameDto">reserveringsid, of er schade is, eventueel een foto hiervan en de gereden kms</param>
        /// <returns>niets, maar stuurd een factuur naar de gebruiker</returns>
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
        
                var prijsOverzicht = await _kostenService.BerekenTotalePrijs(innameDto.ReserveringId, innameDto.IsSchade, innameDto.GeredenKilometers);
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