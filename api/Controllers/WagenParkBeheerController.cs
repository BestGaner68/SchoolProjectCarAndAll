using System.Security.Claims;
using api.Dtos;
using api.Dtos.Account;
using api.Dtos.Verhuur;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/WagenParkBeheer")]
    public class WagenParkBeheerController : ControllerBase
    {
        private readonly IWagenParkUserListService _wagenparkUserListService;
        private readonly IWagenparkService _wagenparkService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWagenParkUserListService _wagenParkUserListService;
        public WagenParkBeheerController(IWagenParkUserListService wagenparkVerzoekService, UserManager<AppUser> userManager, IWagenparkService wagenparkService, IWagenParkUserListService wagenParkUserListService)
        {
            _wagenparkUserListService = wagenparkVerzoekService;
            _userManager = userManager;
            _wagenparkService = wagenparkService;
            _wagenParkUserListService = wagenParkUserListService;
        }
 

        [Authorize]
        [HttpGet("GetAllWagenParkUsers")]
        public async Task<IActionResult> GetAllUserInWagenPark()
        {
            var CurrentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(CurrentWagenparkBeheerder))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            List<AppUser> UsersInWagenPark = await _wagenparkUserListService.GetAllUsersInWagenPark(CurrentWagenparkBeheerder);
            if (!UsersInWagenPark.Any()){
                return BadRequest(new {message = "Er zijn geen gebruikers gevonden in uw WagenPark"});
            }
            var ToDto = UserDtoMapper.MapToUserDtos(UsersInWagenPark);
            return Ok(ToDto);
        }
        
        [Authorize  ]
        [HttpPut("NodigGebruikerUitVoorWagenpark")]
        public async Task<IActionResult> NodigGebruikerUitVoorWagenpark([FromBody]NodigUitDto nodigUitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var WagenParkBeheerderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (WagenParkBeheerderId == null)
            {
                return Unauthorized();
            }
            var succes = await _wagenparkUserListService.StuurInvite(nodigUitDto.Email, WagenParkBeheerderId);
            if (!succes){
                return BadRequest (new {message = "Er is iets misgegaan bij het uitnodigen van de gebruiker"});
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("RemoveUserFromWagenPark")]
        public async Task<IActionResult> RemoveUserFromWagenPark([FromBody] string AppUserId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserId))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            await _wagenparkUserListService.VerwijderGebruiker(UserId);
            return Ok("De gebruiker is verwijderd uit uw wagenPark");
        }

        [Authorize]
        [HttpGet("GetOverzicht")]
        public async Task<IActionResult> GetOverzicht()
        {
            var CurrentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(CurrentWagenparkBeheerder))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            var overzicht = await _wagenparkUserListService.GetOverzicht(CurrentWagenparkBeheerder);
            if (!overzicht.Any())
            {
                return BadRequest("Geen Reserveringen gevonden binnen uw WagenPark");
            }
            return Ok(overzicht);
        }
    }
}