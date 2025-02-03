using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{   
    [Route("api/Reserveringen")]
    public class ReserveringController : ControllerBase
    {
        private readonly IReserveringService _reserveringService;
        private readonly IKostenService _kostenService;
        private readonly ILogger<ReserveringController> _logger;

        public ReserveringController(IReserveringService reserveringService, IKostenService kostenService, ILogger<ReserveringController> logger)
        {
            _reserveringService = reserveringService;
            _kostenService = kostenService;
            _logger = logger;
        }

        /// <summary>
        /// backofficemedewerker kunnen deze methode gebruiken om een verhuurverzoek goed te keuren en te veranderen in een reservering, de gebruiker krijgt een email
        /// </summary>
        /// <param name="verzoekId">Id van het verzoek</param>
        /// <returns>niets, stuurt een email</returns>
        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpPost("KeurVerzoekGoed/{verzoekId}")]
        public async Task<IActionResult> KeurVerzoekGoed([FromRoute] int verzoekId)
        {
            try
            {
                var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized(new { message = "Gebruiker is niet geautoriseerd." });
                }
        
                var result = await _reserveringService.AcceptVerhuurVerzoek(verzoekId);
                if (!result)
                {
                    return NotFound(new { message = "Er is geen verhuurverzoek gevonden of een andere fout opgetreden." });
                }
        
                return Ok(new { message = "Verzoek is succesvol goedgekeurd." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }
        
        /// <summary>
        /// keur het verzoek af en maakt GEEN reservering, status wordt veranderd. email wordt gestuurd met de reden voor weigering 
        /// </summary>
        /// <param name="weigerVerhuurVerzoekDto">verhuurverzoekid en de weiger reden</param>
        /// <returns>niets, stuurt email</returns>
        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpPost("KeurVerzoekAf")] //methode voor weigeren verhuurverzoek
        public async Task<IActionResult> KeurVerzoekAf([FromRoute] WeigerVerhuurVerzoekDto weigerVerhuurVerzoekDto )
        {
            try
            {
                var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized(new { message = "Gebruiker is niet geautoriseerd." });
                }
        
                var result = await _reserveringService.WeigerVerhuurVerzoek(weigerVerhuurVerzoekDto);
                if (!result)
                {
                    return NotFound(new { message = "Er is geen verhuurverzoek gevonden of een andere fout opgetreden." });
                }
        
                return Ok(new { message = "Verhuurverzoek is succesvol afgekeurd." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }

        /// <summary>
        /// Returned alle reserveringen uit de db die nog uitgegeven moeten worden, gebaseerd op status
        /// </summary>
        /// <returns>alle reserveringen uit de db die nog uitgegeven moeten worden</returns>
        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpGet("GetAllReserveringen")] 
        public async Task<IActionResult> GetAllReserveringen()
        {
            try
            {
                var reserveringen = await _reserveringService.GetAll();
                if (reserveringen.Count == 0)
                {
                    return NotFound(new { message = "Er zijn momenteel geen reserveringen." });
                }
                return Ok(reserveringen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van alle reserveringen.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }


        /// <summary>
        /// Returned de reservering gebaseerd op het id
        /// </summary>
        /// <param name="reserveringId">id van reservering</param>
        /// <returns>de reservering</returns>
        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpGet("GetReserveringById/{reserveringId}")] //methode returned de reservering informatie gebaseerd op id
        public async Task<IActionResult> GetReserveringById([FromRoute] int reserveringId)
        {
            try
            {
                var reservering = await _reserveringService.GetById(reserveringId);
                if (reservering == null)
                {
                    return NotFound(new { message = $"Er is geen reservering gevonden met ReserveringId {reserveringId}" });
                }
                return Ok(reservering);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van reservering.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
        

        /// <summary>
        /// gebruiker kan zijn huurgeschiedenis inzien met deze methode, het check alle reserveringen die op de gebruikers naam staan
        /// </summary>
        /// <returns>een overzicht van de reserveringen</returns>
        [Authorize]
        [HttpGet("ViewHuurGeschiedenis")] 
        public async Task<IActionResult> ViewHuurGeschiedenis()
        {
            try
            {
                var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized(new { message = "JWT Token is niet meer in gebruik" });
                }

                var reserveringen = await _reserveringService.GetMyReserveringen(appUserId);
                if (!reserveringen.Any())
                {
                    return NotFound(new { message = "Geen reserveringen gevonden voor deze gebruiker." });
                }

                var huurgeschiedenis = new List<HuurGeschiedenisDto>();
                foreach (var reservering in reserveringen)
                {
                    var huurGeschiedenisDto = await _reserveringService.GetHuurGeschiedenis(reservering);
                    huurgeschiedenis.Add(huurGeschiedenisDto);
                }

                return Ok(huurgeschiedenis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van huurgeschiedenis.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
        /// <summary>
        /// gebruikers mogen nog data aanpassen een week vantevoren met deze methode, als alle data klopt kan de gebruiker zijn voertuig of de data aanpassen
        /// </summary>
        /// <param name="wijzigReserveringDto">data id of allebei die veranderd moeten worden en het id van de reservering</param>
        /// <returns>niets</returns>
        [Authorize]
        [HttpPut("WijzigReservering")] 
        public async Task<IActionResult> WijzigReservering([FromBody] WijzigReserveringDto wijzigReserveringDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultaat = await _reserveringService.WijzigReservering(wijzigReserveringDto);
                if (resultaat)
                {
                    return Ok(new { message = "Reservering succesvol bijgewerkt." });
                }
                return BadRequest(new { message = "Het bijwerken van de reservering is mislukt." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het wijzigen van reservering.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }

        /// <summary>
        /// hiermee kan de gebruiker minimaal een week van tevoren zijn reservering verwijderen
        /// </summary>
        /// <param name="reserveringId">id van de reservering</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("VerwijderReservering/{reserveringId}")] 
        public async Task<IActionResult> VerwijderReservering([FromRoute] int reserveringId)
        {
            try
            {
                var succes = await _reserveringService.VerwijderReservering(reserveringId);
                if (!succes)
                {
                    return BadRequest(new { message = "Er is iets misgegaan bij het verwijderen van de reservering." });
                }
                return Ok(new { message = "Verzoek succesvol verwijderd." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het verwijderen van reservering.");
                return StatusCode(500, new { message = "Er is een interne fout opgetreden", details = ex.Message });
            }
        }
    }
}