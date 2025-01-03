using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{   
    [Route("api/VerhuurVerzoekBehandelen")]
    public class VerhuurVerzoekBehandelController : ControllerBase
    {
        private readonly IReserveringService _reserveringService;
        public VerhuurVerzoekBehandelController(IReserveringService reserveringService){
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllReserveringen (){
            var Reserveringen = await _reserveringService.GetAll();
            if (Reserveringen == null){
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
    }
}