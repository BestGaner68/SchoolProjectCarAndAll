using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Mapper;
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
            return Ok(Verzoeken.Select(x => x.ToVerhuurVerzoekDto())); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var verhuurVerzoekByID = await _verhuurVerzoekRepo.GetByIdAsync(id);
            if (verhuurVerzoekByID == null)
            {
                return NotFound();
            }
            return Ok(verhuurVerzoekByID);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VerhuurVerzoekRequestDto verhuurVerzoekDto)
        {
            var verhuurVerzoekModel = verhuurVerzoekDto.ToVerhuurVerzoekFromDto();
            await _verhuurVerzoekRepo.CreateAsync(verhuurVerzoekModel);
            return CreatedAtAction(nameof(GetById), new {id = verhuurVerzoekModel.VerhuurVerzoekId}, verhuurVerzoekModel.ToVerhuurVerzoekDto());
        }
        
        
    }
}