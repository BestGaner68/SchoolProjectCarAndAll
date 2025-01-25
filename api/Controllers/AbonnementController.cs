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

        [HttpGet("GetAllAbonnementen")]
        public async Task<IActionResult> GetAllAbonnementen()
        {
            var abonnementen = await _abonnementService.GetAllAbonnementen();
            return Ok(abonnementen);
        }

        [HttpPost("wijzig-abonnement-user")]
        public async Task<IActionResult> WijzigAbonnementUser([FromBody] IdDto NieuwAbonnementId)
        {
            try
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (UserId == null)
                {
                    return Unauthorized(new {message = "Gebruiker niet geauthoriseerd."});
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
        
        [HttpPost("wijzig-abonnement-wagenpark")]
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

        
        [HttpPut("ExtentCurrentAbonnement")]
        public async Task<IActionResult> ExtentCurrentAbonnement([FromBody] AbonnementWeizigDto abonnementweizigDto)
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (AppUserId == null){
                    return Unauthorized(new {message = "Gebruiker niet geauthoriseerd."});
                }
                var CurrentWagenPark = await _wagenparkService.GetBeheerdersWagenPark(AppUserId);
                if (CurrentWagenPark == null){
                    return Unauthorized(new {message = "Gebruiker is geen eigenaar van een wagenpark"});
                }
                var succes = await _abonnementService.ExtentCurrentAbonnement(CurrentWagenPark.WagenParkId);
                if (!succes)
                {
                    return BadRequest(new {message = "Er is iets misgegaan"});
                }
                return Ok(new {message = "succesvol abonnement verlengt met 3 maanden"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }

        [HttpGet("GetCurrentAbonnement")]
        public async Task<IActionResult> GetCurrentAbonnement()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new {message = "Gebruiker niet geauthoriseerd."});
            var abonnement = await _abonnementService.GetUserAbonnement(userId);
            return Ok(abonnement);
        }

    }
}