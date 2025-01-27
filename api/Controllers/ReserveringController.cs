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

        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpPost("KeurVerzoekGoed/{verzoekId}")] //methode voor goedkeuren verhuurverzoek
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
        
        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpPost("KeurVerzoekAf/{verzoekId}")] //methode voor weigeren verhuurverzoek
        public async Task<IActionResult> KeurVerzoekAf([FromRoute] int verzoekId)
        {
            try
            {
                var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized(new { message = "Gebruiker is niet geautoriseerd." });
                }
        
                var result = await _reserveringService.WeigerVerhuurVerzoek(verzoekId);
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

        [Authorize(Roles = $"{Rollen.BackendWorker},{Rollen.FrontendWorker}")]
        [HttpGet("GetAllReserveringen")] //methode returned een lijst met alle reserveringen die verwerkt moeten worden
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

        [Authorize]
        [HttpGet("ViewHuurGeschiedenis")] //methode voor het inzien van verhuurgeschiedenis voor gebruikers
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

        [Authorize]
        [HttpPut("WijzigReservering")] //methode voor het aanpassen van data of voertuig van een reservering als het nog mag (1 week van tevoren)
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

        [Authorize]
        [HttpDelete("VerwijderReservering/{reserveringId}")] //methode verwijderd de reservering van de gebruiker (minimaal 1 week van tevoren)
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