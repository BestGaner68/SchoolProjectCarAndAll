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
        public async Task<IActionResult> GetAll()
        {
            return Ok("AddAbonnement endpoint hit, but logic not implemented.");
        }

        [HttpPut("AddAbonnement")]
        public async Task<IActionResult> AddUserAbonnement(int AbonnementId)
        {
            return Ok("AddAbonnement endpoint hit, but logic not implemented.");
        }

        [HttpPut("UpdateAbonnement")]
        public async Task<IActionResult> updateUserAbonnement(int AbonnementId)
        {
            return Ok("UpdateAbonnement endpoint hit, but logic not implemented.");
        }


    }
}