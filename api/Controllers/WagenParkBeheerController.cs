using System.Security.Claims;
using api.Interfaces;
using api.Mapper;
using api.Migrations;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/WagenParkBeheer")]
    public class WagenParkBeheerController : ControllerBase
    {
        private readonly IWagenparkVerzoekService _wagenparkVerzoekService;
        public WagenParkBeheerController(IWagenparkVerzoekService wagenparkVerzoekService)
        {
            _wagenparkVerzoekService = wagenparkVerzoekService;
        }

        [Authorize]
        [HttpGet("GetAllVerzoeken")]
        public async Task <IActionResult> GetAllVerzoeken()  
        {
            var CurrentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var verzoeken = await _wagenparkVerzoekService.GetAllVerzoeken(CurrentWagenparkBeheerder);
            if (verzoeken == null || !verzoeken.Any())
            {
                return NotFound($"No requests found for WagenPark with ID .");
            }
            var verzoekenToDto = UserDtoMapper.MapToWagenParkDtos(verzoeken);
                return Ok(verzoekenToDto);
        }

        [HttpPost("AddUserToWagenPark")]
        public async Task<IActionResult> AdduserToWagenPark([FromBody] int verzoekId)
        {
            try
            {
                var succes = await _wagenparkVerzoekService.AcceptUserRequest(verzoekId);
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

        [HttpPost("DenyUserToWagenPark")]
        public async Task<IActionResult> DenyUserToWagenPark([FromBody] int verzoekId)
        {
            try
            {
            var succes = await _wagenparkVerzoekService.DenyUserRequest(verzoekId);
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

        [HttpGet("GetAllWagenParkUsers/")]
        public async Task<IActionResult> GetAllUserInWagenPark()
        {
            var CurrentWagenparkBeheerder = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<AppUser> UsersInWagenPark = await _wagenparkVerzoekService.GetAllUsers(CurrentWagenparkBeheerder);
            if (!UsersInWagenPark.Any()){
                return BadRequest(new {message = "Er zijn geen gebruikers gevonden in uw WagenPark"});
            }
            var ToDto = UserDtoMapper.MapToUserDtos(UsersInWagenPark);
            return Ok(ToDto);
        }

    }
}