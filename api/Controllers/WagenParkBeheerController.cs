using System.Security.Claims;
using api.Dtos;
using api.Dtos.Account;
using api.Dtos.Verhuur;
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
        private readonly IWagenparkVerzoekService _wagenparkVerzoekService;
        private readonly IWagenparkService _wagenparkService;
        private readonly UserManager<AppUser> _userManager;
        public WagenParkBeheerController(IWagenparkVerzoekService wagenparkVerzoekService, UserManager<AppUser> userManager, IWagenparkService wagenparkService)
        {
            _wagenparkVerzoekService = wagenparkVerzoekService;
            _userManager = userManager;
            _wagenparkService = wagenparkService;
        }

        [Authorize]
        [HttpGet("GetAllVerzoeken")]
        public async Task <IActionResult> GetAllVerzoeken()  
        {
            var CurrentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(CurrentWagenparkBeheerder))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            var verzoeken = await _wagenparkVerzoekService.GetAllVerzoeken(CurrentWagenparkBeheerder);
            if (verzoeken.Count == 0)
            {
                return NotFound($"Geen wagenparkVerzoeken gevonden");
            }
            var VerzoekenLijst = new List<WagenParkDataDto>();
            foreach (WagenParkVerzoek currentVerzoek in verzoeken)
            {
                var AppUser = await _userManager.FindByIdAsync(currentVerzoek.AppUserId);
                var Dto = UserDtoMapper.MapToWagenParkDto(currentVerzoek, AppUser);
                VerzoekenLijst.Add(Dto);
            }
            return Ok(VerzoekenLijst);           
        }

        [Authorize]
        [HttpPost("AddUserToWagenPark")]
        public async Task<IActionResult> AdduserToWagenPark([FromBody] IdDto verzoekId)
        {
            try
            {
                var CurrentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var succes = await _wagenparkVerzoekService.AcceptUserRequest(verzoekId.Id, CurrentWagenparkBeheerder);
                if (succes)
                {
                    return Ok("User added to Wagenpark successfully.");
                }
                return BadRequest("Failed to add user to Wagenpark.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("DenyUserToWagenPark")]
        public async Task<IActionResult> DenyUserToWagenPark([FromBody] IdDto verzoekId)
        {
            try
            {
            var CurrentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(CurrentWagenparkBeheerder))
            {
                return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
            }
            var succes = await _wagenparkVerzoekService.DenyUserRequest(verzoekId.Id, CurrentWagenparkBeheerder);
            if (succes)
            {
                return Ok("User request denied.");
            }
            return BadRequest("Failed to deny user request.");
            }
            catch (Exception ex){
                return StatusCode(500, $"bruh moment: {ex.Message}");
            }
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
            List<AppUser> UsersInWagenPark = await _wagenparkService.GetAllUsers(CurrentWagenparkBeheerder);
            if (!UsersInWagenPark.Any()){
                return BadRequest(new {message = "Er zijn geen gebruikers gevonden in uw WagenPark"});
            }
            var ToDto = UserDtoMapper.MapToUserDtos(UsersInWagenPark);
            return Ok(ToDto);
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
            var Result = await _wagenparkVerzoekService.RemoveUser(AppUserId, UserId);
            if (!Result)
            {
                return BadRequest("Er is iets misgegaan");
            }
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
            var overzicht = await _wagenparkVerzoekService.GetOverzicht(CurrentWagenparkBeheerder);
            if (!overzicht.Any())
            {
                return BadRequest("Geen Reserveringen gevonden binnen uw WagenPark");
            }
            return Ok(overzicht);
        }
    }
}