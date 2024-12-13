using api.Interfaces;
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
        [HttpGet("GetAllVerzoeken")]
        public async Task <IActionResult> GetAllVerzoeken([FromRoute]int id)
        {
            return Ok("");
        }


        [HttpPost]
        public async Task<IActionResult> AdduserToWagenPark(AppUser user, wagenpark wagenpark )
        {
            // await _wagenparkVerzoekService.AddUserToWagenPark(user.id, wagenpark.wagenparkid);
            return Ok("its ok");
        }
        [HttpGet("GetAllWagenParkUsers")]
        public async Task<IActionResult> GetAllUserInWagenPark([FromRoute] int wagenparkid)
        {
            List<AppUser> UsersInWagenPark = await _wagenparkVerzoekService.GetAllUsers(wagenparkid);
            return Ok(UsersInWagenPark);
        }

    }
}