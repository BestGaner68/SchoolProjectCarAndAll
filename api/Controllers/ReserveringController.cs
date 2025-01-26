using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public ReserveringController(IReserveringService reserveringService, IWagenparkService wagenparkService){
            _reserveringService = reserveringService;
        }

        [HttpPost("KeurVerzoekGoed/{verzoekId}")]
        public async Task<IActionResult> KeurVerzoekGoed ([FromRoute]int verzoekId){
            var result = await _reserveringService.AcceptVerhuurVerzoek(verzoekId);
            if (!result){
                return NotFound("Er is geen verhuurverzoek gevonden of een andere fout opgetreden");
            }
            return Ok("VerhuurVerzoek is goedgekeurt en toegevoegt aan de reserveringen tabel");

        }
        [HttpPost("keurVerzoekAf/{verzoekId}")]
        public async Task<IActionResult> KeurVerzoekAf ([FromRoute]int verzoekId){
            var result = await _reserveringService.WeigerVerhuurVerzoek(verzoekId);
            if (!result){
                return NotFound("Er is geen verhuurverzoek gevonden of een andere fout opgetreden");
            }
            return Ok("VerhuurVerzoek is afgekeurt");
        }
        [HttpGet("GetAllReserveringen")]
        public async Task<IActionResult> GetAllReserveringen (){
            var Reserveringen = await _reserveringService.GetAll();
            if (!Reserveringen.Any()){
                return NotFound("Er zijn momenteel geen Reserveringen");
            }
            return Ok(Reserveringen);
        }

        [HttpGet("GetReserveringById/{ReserveringId}")]
        public async Task<IActionResult> GetReserveringById ([FromRoute]int ReserveringId){
            var Reservering = await _reserveringService.GetById(ReserveringId);
            if (Reservering == null){
                return NotFound($"Er is geen reservering gevonden met ReserveringId {ReserveringId}");
            }
            return Ok(Reservering);
        }
        [Authorize]
        [HttpGet("ViewHuurGeschiedenis")]
        public async Task<IActionResult> ViewMyGeschiedenis()
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
                }
                var Reserveringen = await _reserveringService.GetMyReserveringen(AppUserId);
                if (Reserveringen.Count == 0)
                {
                    return NotFound(new { message = "Geen reserveringen gevonden voor deze gebruiker." });
                }
                var huurgeschiedenis = new List<HuurGeschiedenisDto>();
                foreach (var Reservering in Reserveringen)
                {
                    
                    var ToevoegenHuurGeschiedenis = await _reserveringService.GetHuurGeschiedenis(Reservering);
                    huurgeschiedenis.Add(ToevoegenHuurGeschiedenis);
                }
                return Ok(huurgeschiedenis);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden." });
            }
        }

        [HttpPut("WijzigReservering/{reserveringId}")]
        public async Task<IActionResult> WijzigReservering([FromBody] WijzigReserveringDto wijzigReserveringDto)
        {
            try
            {
                var result = await _reserveringService.WijzigReservering(wijzigReserveringDto);
                if (result)
                {
                    return Ok("Reservering successfully updated.");
                }
                return BadRequest("Failed to update reservering.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("VerwijderReservering/{reserveringId}")]
        public async Task<IActionResult> VerwijderReservering([FromRoute] int reserveringId)
        {
            try
            { 
                var succes = await _reserveringService.VerwijderReservering(reserveringId);
                if (!succes)
                {
                    return BadRequest(new { Message = "Er is iets misgegaan" });
                }
                return Ok("Verzoek succesvol verwijderd");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message }); 
            }
        }
    }
}