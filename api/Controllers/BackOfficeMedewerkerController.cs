using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.Account;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Dtos.Voertuig;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/BackOfficeMedewerker")]
    public class BackOfficeMedewerkerController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IWagenparkService _wagenparkService;
        private readonly IVoertuigService _voertuigService;
        private readonly IReserveringService _reserveringService;
        public BackOfficeMedewerkerController(UserManager<AppUser> userManager, ITokenService tokenService, IWagenparkService wagenparkService, IVoertuigService voertuigService, IReserveringService reserveringService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _wagenparkService = wagenparkService;
            _voertuigService = voertuigService;
            _reserveringService = reserveringService;
        }

        [HttpPost("registerBackendAndFrontend")]
        public async Task<IActionResult> RegisterBackendAndFrontend([FromBody] RegisterBackOrFrontEndWorkerDto registerOfficeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var validRoles = new[] { "BackendWorker", "FrontendWorker" };
                if (!validRoles.Contains(registerOfficeDto.TypeAccount))
                {
                    return BadRequest("Verkeerde Rol, Mogelijkheden: BackendWorker, FrontendWorker.");
                }

                var appUser = new AppUser
                {
                    UserName = registerOfficeDto.Username,
                    Email = registerOfficeDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerOfficeDto.Password);

                if (!createdUser.Succeeded)
                {
                    return StatusCode(500, createdUser.Errors);
                }

                IdentityResult roleResult;
                if (registerOfficeDto.TypeAccount == "backendWorker")
                {
                    roleResult = await _userManager.AddToRoleAsync(appUser, "backendWorker");
                }
                else
                {
                    roleResult = await _userManager.AddToRoleAsync(appUser, "frontendWorker");
                }
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, roleResult.Errors);
                }

                return Ok(
                    new NewUserDto
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPut("BlokkeerdVoertuig")]
        public async Task<IActionResult> BlokkeerVoertuig (int voertuigId, string Opmerking){
            var result = await _voertuigService.BlokkeerVoertuig(voertuigId, Opmerking);
            if (!result){
                return BadRequest($"Geen voertuig gevonden met id {voertuigId}");
            }
            return Ok($"status van het voertuig met id {voertuigId} is veranderd naar {VoertuigStatussen.Geblokkeerd}");
        }

        [HttpPut("DeblokkeerVoertuig")]
        public async Task<IActionResult> DeblokkeerVoertuig (int voertuigId){
            var result = await _voertuigService.DeBlokkeerVoertuig(voertuigId);
            if (!result){
                return BadRequest($"Geen voertuig gevonden met id {voertuigId} of het voertuig is momenteel niet geblokkeerd");
            }
            return Ok($"status van het voertuig met id {voertuigId} is veranderd naar {VoertuigStatussen.KlaarVoorGebruik}");
        }

        [HttpGet("GetAllSchadeMeldingen")]
        public async Task<IActionResult> GetAllSchadeMeldingen()
        {
            var Schadeformulieren = await _voertuigService.GetAllIngediendeFormulieren();
            if (Schadeformulieren.Count == 0)
            {
                return NotFound(new {message = "Geen schadeFormulieren gevonden die nog niet behandeld zijn"});
            }
            var result = Schadeformulieren.Select(f => new
            {
                f.SchadeFormulierID,
                f.VoertuigId,
                f.Schade,
                f.SchadeDatum,
                f.ReparatieOpmerking,
                Foto = f.SchadeFoto != null ? Convert.ToBase64String(f.SchadeFoto) : null
            }).ToList();
            return Ok(result);
        }

        [HttpPut("BehandelSchadeMelding")]
        public async Task<IActionResult> BehandelSchadeMelding (SchadeMeldingBehandelDto schadeMeldingBehandelDto)
        {
            if (!ModelState.IsValid){
                return BadRequest(new {message = "ongeldige invoer"});
            }
            var succes = await _voertuigService.BehandelSchadeMelding(schadeMeldingBehandelDto.SchadeFormulierId, schadeMeldingBehandelDto.ReparatieOpmerking);
            if(!succes){
                return BadRequest(new {message = "Er is iets misgegaan bij het verwerken van de schade."});
            }
            return Ok(new { message = "Reparatie is succesvol afgehandeld en de voertuigstatus is aangepast."});
        }

        [HttpPut("MeldSchade")]
        public async Task<IActionResult> MeldSchade(MeldSchadeDto meldSchadeDto) 
        {
            var result = await _reserveringService.MeldSchadeVanuitVoertuigId(meldSchadeDto.VoertuigId, meldSchadeDto.Schade, meldSchadeDto.SchadeFoto);
            if (!result)
            {
                return BadRequest(new {message = "Er is iets misgegaan"});
            }
            return Ok(new {message = "Succes schade gemeld bij voertuig"});
        }
    
        [HttpPost("AddVoertuig")]
        public async Task<IActionResult> AddVoertuig([FromBody] NieuwVoertuigDto nieuwVoertuigDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Niet alle data is juist opgeleverd.", errors = ModelState });
            }
            var result = await _voertuigService.CreeerNieuwVoertuig(nieuwVoertuigDto);
            if (!result){
                return BadRequest(new {message = "Er is iets misgegaan bij het aanmaken van het voertuig."});
            }
            return Ok(new {message = $"Succesvol nieuw voertuig met kenteken: {nieuwVoertuigDto.Kenteken} toegevoegt."});
        }
        
        [HttpDelete("VerwijderVoertuig")]
        public async Task<IActionResult> VerwijderVoertuig([FromBody]IdDto voertuigId)
        {
            var result = await _voertuigService.VerwijderVoertuig(voertuigId.Id);
            if (!result){
                return BadRequest(new {message = $"Er is iets misgegaan bij het verwijderen van het voertuig, waarschijn is er geen voertuig met id {voertuigId}."});
            }
            return Ok(new {message = $"Voertuig met id {voertuigId} is succesvol verwijderd"});
        }
        
        [HttpPut("WijzigVoertuig")]
        public async Task<IActionResult> WijzigVoertuig([FromBody] WeizigVoertuigDto weizigVoertuigDto)
        {
            var result = await _voertuigService.WeizigVoertuig(weizigVoertuigDto);
            if (!result){
                return BadRequest(new {message = "Er is iets misgegaan bij het wijzigen van het voertuig."});
            }
            return Ok(new {message = $"Voertuig met id {weizigVoertuigDto.VoertuigId} is succesvol geweizigd."});
        }

        [HttpGet("GetAllNieuwWagenParkVerzoeken")]
        public async Task<IActionResult> GetAllNieuwWagenParkVerzoeken()
        {
            var verzoeken = await _wagenparkService.GetAllWagenparkVerzoekenAsync();
            if (verzoeken == null || verzoeken.Count == 0)
            {
                return NotFound("Er zijn geen nieuwe verzoeken.");
            }
    
            return Ok(verzoeken);
        }
    
        
        [HttpPut("AcceptVerzoek")]
        public async Task<IActionResult> AcceptVerzoek([FromBody] IdDto idDto)
        {
            try
            {
                var wagenPark = await _wagenparkService.AcceptNieuwWagenParkVerzoek(idDto.Id);
                return Ok(wagenPark);
            }
            catch (Exception ex)
            {
                return BadRequest($"Er is iets misgegaan: {ex.Message}");
            } 
        }
    
        [HttpPut("WeigerVerzoek")]
        public async Task<IActionResult> WeigerVerzoek([FromBody]WeigerNieuwWagenParkVerzoekDto Dto)
        {
            var result = await _wagenparkService.WeigerNieuwWagenParkVerzoek(Dto);
            if (!result)
            {
                return BadRequest("Er is een fout opgetreden bij het weigeren van het verzoek.");
            }
    
            return NoContent(); 
        }

    }   
}