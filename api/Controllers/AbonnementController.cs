using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/Abonnement")]
    [ApiController]
    public class AbonnementController : ControllerBase
    {
        private readonly IAbonnementService _abonnementService;
        public AbonnementController(IAbonnementService abonnementService)
        {
            _abonnementService = abonnementService;
        }

        [HttpGet("GetAllAbonnementen")]
        public async Task<IActionResult> GetAllAbonnementen()
        {
            var AllAbonnementen = await _abonnementService.getAllAbonnementen();
            return Ok(AllAbonnementen);
        }

        [HttpPut("ChangeAbonnement")]
        public async Task<IActionResult> ChangeAbonnement(int abonnementId, string appUserId)
        {
            var success = true;//await _abonnementService.KiesAbonnement(abonnementId, appUserId);
            if (!success)
            {
                return BadRequest(new { message = "Er is een probleem opgetreden met het wijzigen van het abonnement." });
            }
            return Ok(new { message = "Abonnement succesvol gewijzigd." });
        }
    }
}