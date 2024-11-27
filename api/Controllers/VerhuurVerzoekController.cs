using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Migrations;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace api.Controllers
{
    [Route("api/verhuurVerzoek")]
    public class VerhuurVerzoekController : ControllerBase
    {
        private readonly IVerhuurVerzoekService _verhuurVerzoekRepo;
        public VerhuurVerzoekController(IVerhuurVerzoekService verhuurVerzoekRepo)
        {
            _verhuurVerzoekRepo = verhuurVerzoekRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Verzoeken = await _verhuurVerzoekRepo.GetAllAsync();
            return Ok(Verzoeken);
        }
        
    }
}