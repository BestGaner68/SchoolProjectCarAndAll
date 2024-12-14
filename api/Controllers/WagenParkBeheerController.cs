using api.Interfaces;
using api.Mapper;
using api.Migrations;
using api.Models;
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
        [HttpGet("GetAllVerzoeken/{id}")]
        public async Task <IActionResult> GetAllVerzoeken([FromRoute]int id)  
        {
        var verzoeken = await _wagenparkVerzoekService.GetAllVerzoeken(id);
        if (verzoeken == null || !verzoeken.Any())
        {
            return NotFound($"No requests found for WagenPark with ID {id}.");
        }
        var verzoekenToDto = UserDtoMapper.MapToWagenParkDtos(verzoeken);
            return Ok(verzoekenToDto);
        }
        


        [HttpPost("AddUserToWagenPark")]
        public async Task<IActionResult> AdduserToWagenPark([FromBody]WagenParkVerzoek verzoek)
        {
            await _wagenparkVerzoekService.AddUserToWagenPark(verzoek);
            return Ok($"User {verzoek.appUser.UserName} succesvol toegevoegt aan het wagenpark van: {verzoek.wagenPark.Bedrijfsnaam}");
        }

        [HttpPost("DenyUserToWagenPark")]
        public async Task<IActionResult> DenyUserToWagenPark([FromBody]WagenParkVerzoek verzoek){
            await _wagenparkVerzoekService.RemoveVerzoek(verzoek);
            return Ok($"User {verzoek.appUser.UserName} Niet toegevoegt aan wagenpark");
        } 

        [HttpGet("GetAllWagenParkUsers/{wagenparkid}")]
        public async Task<IActionResult> GetAllUserInWagenPark([FromRoute] int wagenparkid)
        {
            List<AppUser> UsersInWagenPark = await _wagenparkVerzoekService.GetAllUsers(wagenparkid);
            var ToDto = UserDtoMapper.MapToUserDtos(UsersInWagenPark);
            return Ok(ToDto);
        }

    }
}