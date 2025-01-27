using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/voertuigen")]
    [ApiController]
    public class VoertuigController : ControllerBase
    {
        private readonly IVoertuigService _voertuigService;

        public VoertuigController(IVoertuigService voertuigService)
        {
            _voertuigService = voertuigService;
        }

        [HttpGet("AllVoertuigen")] //returned all voertuigen
        public async Task<IActionResult> GetAllVoertuigen()
        {
            try
            {
                var voertuigen = await _voertuigService.GetAllVoertuigen();
                if (!voertuigen.Any())
                {
                    return NotFound(new { message = "Geen voertuigen gevonden." });
                }
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }

        [HttpGet("GetVoertuigByMerk")] //returned voertuig gebaseerd op merk, gebruik in frontend
        public async Task<IActionResult> GetVoertuigenByMerk([FromQuery] string voertuigMerk)
        {
            try
            {
                if (string.IsNullOrEmpty(voertuigMerk))
                {
                    return BadRequest(new { message = "Voertuigmerk mag niet leeg zijn." });
                }

                var voertuigen = await _voertuigService.GetVoertuigenByMerk(voertuigMerk);
                if (!voertuigen.Any())
                {
                    return NotFound(new { message = $"Geen voertuigen gevonden met merk: {voertuigMerk}" });
                }
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }


        [HttpGet("GetVoertuigBySoort")] //returned voertuig gebaseerd op soort, gebruik in frontend
        public async Task<IActionResult> GetVoertuigenBySoort([FromQuery] string voertuigSoort)
        {
            try
            {
                if (string.IsNullOrEmpty(voertuigSoort))
                {
                    return BadRequest(new { message = "Voertuigsoort mag niet leeg zijn." });
                }

                var voertuigen = await _voertuigService.GetVoertuigenBySoort(voertuigSoort);
                if (!voertuigen.Any())
                {
                    return NotFound(new { message = $"Geen voertuigen gevonden met soort: {voertuigSoort}" });
                }
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }
        
        [HttpGet("GetVoertuigByDate")] //returned voertuigen die in de periode beschikbaar zijn
        public async Task<IActionResult> GetVoertuigenByDate([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                if (!startDate.HasValue || !endDate.HasValue)
                {
                    return BadRequest(new { message = "Start- en einddatum zijn verplicht." });
                }

                if (startDate > endDate)
                {
                    return BadRequest(new { message = "Startdatum kan niet later zijn dan de einddatum." });
                }

                var dateRangeRequest = new Dtos.ReserveringenEnSchade.DatumDto
                {
                    StartDate = startDate.Value,
                    EndDate = endDate.Value
                };

                var beschikbareVoertuigen = await _voertuigService.GetVoertuigenByDate(dateRangeRequest);
                if (!beschikbareVoertuigen.Any())
                {
                    return NotFound(new { message = "Geen voertuigen beschikbaar binnen de opgegeven periode." });
                }
                return Ok(beschikbareVoertuigen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }
    }
}