using System.Security.Claims;
using api.Dtos;
using api.Dtos.Account;
using api.Dtos.Verhuur;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/WagenParkBeheer")]
    [ApiController]
    public class WagenParkBeheerController : ControllerBase
    {
        private readonly IWagenParkUserListService _wagenParkUserListService;


        public WagenParkBeheerController(IWagenParkUserListService wagenparkUserListService)
        {
            _wagenParkUserListService = wagenparkUserListService;
        }

        [Authorize]
        [HttpGet("GetAllWagenParkUsers")] //returned all users geregistreerd in gebruikers wagenpark
        public async Task<IActionResult> GetAllUserInWagenPark()
        {
            try
            {
                var currentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentWagenparkBeheerder))
                {
                    return Unauthorized(new { message = "JWT-token is niet meer geldig." });
                }

                var usersInWagenPark = await _wagenParkUserListService.GetAllUsersInWagenPark(currentWagenparkBeheerder);
                if (!usersInWagenPark.Any())
                {
                    return NotFound(new { message = "Er zijn geen gebruikers gevonden in uw WagenPark." });
                }

                var dto = UserDtoMapper.MapToUserDtos(usersInWagenPark);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("NodigGebruikerUitVoorWagenpark")] //stuur uitnodig per email naar gewensde gebruiker, daarna kan account worden aangemaakt
        public async Task<IActionResult> NodigGebruikerUitVoorWagenpark([FromBody] NodigUitDto nodigUitDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var wagenParkBeheerderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(wagenParkBeheerderId))
                {
                    return Unauthorized(new { message = "JWT-token is niet meer geldig." });
                }

                var succes = await _wagenParkUserListService.StuurInvite(nodigUitDto.Email, wagenParkBeheerderId);
                if (!succes)
                {
                    return BadRequest(new { message = "Er is iets misgegaan bij het uitnodigen van de gebruiker." });
                }

                return Ok(new { message = $"Gebruiker met e-mail {nodigUitDto.Email} is succesvol uitgenodigd." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("RemoveUserFromWagenPark")] //verwijderd PERMANENT een gebruiker van het wagenpark
        public async Task<IActionResult> RemoveUserFromWagenPark([FromBody] string appUserId)
        {
            try
            {
                if (string.IsNullOrEmpty(appUserId))
                {
                    return BadRequest(new { message = "Gebruikers-ID mag niet leeg zijn." });
                }

                var succes = await _wagenParkUserListService.VerwijderGebruiker(appUserId);
                if (!succes)
                {
                    return NotFound(new { message = "De opgegeven gebruiker is niet gevonden in uw WagenPark." });
                }

                return Ok(new { message = "De gebruiker is succesvol verwijderd uit uw WagenPark." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetOverzicht")] //maakt een overzicht van de huringen van de gebruikers in het wagenpark
        public async Task<IActionResult> GetOverzicht()
        {
            try
            {
                var currentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentWagenparkBeheerder))
                {
                    return Unauthorized(new { message = "JWT-token is niet meer geldig." });
                }

                var overzicht = await _wagenParkUserListService.GetOverzicht(currentWagenparkBeheerder);
                if (overzicht.Count == 0)
                {
                    return NotFound(new { message = "Geen reserveringen gevonden binnen uw WagenPark." });
                }

                return Ok(overzicht);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een interne fout opgetreden.", details = ex.Message });
            }
        }
    }
}