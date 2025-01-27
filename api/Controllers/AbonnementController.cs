using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Verhuur;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/Abonnementen")]
    [ApiController]
    public class AbonnementController : ControllerBase
     {
        private readonly IAbonnementService _abonnementService;
        private readonly IWagenparkService _wagenparkService;

        public AbonnementController(IAbonnementService abonnementService, IWagenparkService wagenparkService)
        {
            _abonnementService = abonnementService;
            _wagenparkService = wagenparkService;
        }
        [HttpGet("GetUserAbonnementen")] //methode returned de abonnement die gebruikers kunnen kiezen
        [Authorize]
        public async Task<IActionResult> GetAllUserAbonnementen()
        {
            try
            {
                var abonnementen = await _abonnementService.GetAllUserAbonnementen();
                return Ok(abonnementen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }
        [HttpGet("GetWagenparkBeheerderAbonnementen")] //methode returned abonnementen die wagenparkbeheerder kunnen kiezen
        [Authorize]
        public async Task<IActionResult> GetWagenparkBeheerderAbonnementen()
        {
            try
            {
                var abonnementen = await _abonnementService.GetAllWagenparkBeheerderAbonnementen();
                return Ok(abonnementen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }
        [HttpPost("wijzig-abonnement-user")] //wordt gebruikt om het abonnement van een user te weizigen
        [Authorize]
        public async Task<IActionResult> WijzigAbonnementUser([FromBody] IdDto NieuwAbonnementId)
        {
            try
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (UserId == null)
                {
                    return Unauthorized(new { message = "Gebruiker niet geauthoriseerd." });
                }
                var success = await _abonnementService.WijzigAbonnementUser(UserId, NieuwAbonnementId.Id);
                if (success)
                {
                    return Ok("Abonnement succesvol gewijzigd.");
                }
                return BadRequest("Abonnement kan niet gewijzigd worden.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }
        [HttpPost("wijzig-abonnement-wagenpark")] //wordt gebruikt om abonnement van een wagenparkbeheerder te weizigen
        [Authorize]
        public async Task<IActionResult> WijzigAbonnementWagenpark([FromBody] AbonnementWeizigDto abonnementWijzigDto)
        {
            try
            {
                var success = await _abonnementService.WijzigAbonnementWagenpark(abonnementWijzigDto.WagenparkId, abonnementWijzigDto.NieuwAbonnementId);
                if (success)
                {
                    return Ok("Wagenpark abonnement succesvol gewijzigd.");
                }
                return BadRequest("Abonnement kan niet gewijzigd worden.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }
        [HttpPut("ExtentCurrentAbonnement")] //wordt gebruikt om de periode van het abonnement te verlengen
        [Authorize]
        public async Task<IActionResult> ExtentCurrentAbonnement([FromBody] AbonnementWeizigDto abonnementweizigDto)
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (AppUserId == null)
                {
                    return Unauthorized(new { message = "Gebruiker niet geauthoriseerd." });
                }
                var CurrentWagenPark = await _wagenparkService.GetBeheerdersWagenPark(AppUserId);
                if (CurrentWagenPark == null)
                {
                    return Unauthorized(new { message = "Gebruiker is geen eigenaar van een wagenpark" });
                }
                var succes = await _abonnementService.ExtentCurrentAbonnement(CurrentWagenPark.WagenParkId);
                if (!succes)
                {
                    return BadRequest(new { message = "Er is iets misgegaan" });
                }
                return Ok(new { message = "Succesvol abonnement verlengd met 3 maanden" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }
        [HttpGet("GetCurrentAbonnement")] //returned het abonnement wat de gebruiker momenteel heeft
        [Authorize]
        public async Task<IActionResult> GetCurrentAbonnement()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized(new { message = "Gebruiker niet geauthoriseerd." });
                var abonnement = await _abonnementService.GetUserAbonnement(userId);
                if (abonnement == null)
                {
                    return NotFound(new { message = "Geen abonnement gevonden voor deze gebruiker." });
                }
                return Ok(abonnement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }
    }
}
