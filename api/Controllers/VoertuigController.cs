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

        /// <summary>
        /// returned elke voertuig uit de db
        /// </summary>
        /// <returns>returned elke voertuig uit de db</returns>
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

        /// <summary>
        /// filter voor voertuigen gebaseerd op merk
        /// </summary>
        /// <param name="voertuigMerk">het op te filteren merk</param>
        /// <returns>de voertuigen van dat merk</returns>
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


        /// <summary>
        /// voor filteren op soort
        /// </summary>
        /// <param name="voertuigSoort">soort van het voertuig</param>
        /// <returns>alle voertuigen van die soort</returns>
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
        
        /// <summary>
        /// returned alle voertuigen die beschikbaar zijn tussen de aangegeven data
        /// </summary>
        /// <param name="startDate">begindatum</param>
        /// <param name="endDate">einddatum</param>
        /// <returns>de voertuigen die tussen die data kunnen worden verhuurd</returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returned alle data en info van een voertuig</returns>
        [HttpGet("GetAllVoertuigDataEnStatus")]
        public async Task<IActionResult> GetAllVoertuigDataEnStatus()
        {
            return Ok(await _voertuigService.GetAllVoertuigen());
        }
    }
}