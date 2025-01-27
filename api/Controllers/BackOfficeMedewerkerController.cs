using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.Account;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Dtos.VoertuigDtos;
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

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPost("registerBackendAndFrontend")] //methode voor het registreren van werknemers CarAndALl
        public async Task<IActionResult> RegisterBackendAndFrontend([FromBody] RegisterBackOrFrontEndWorkerDto registerOfficeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var validRoles = new[] { Rollen.BackendWorker, Rollen.FrontendWorker };
                if (!validRoles.Contains(registerOfficeDto.TypeAccount))
                {
                    return BadRequest($"Verkeerde Rol, Mogelijkheden: {Rollen.BackendWorker}, {Rollen.FrontendWorker}");
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
                if (registerOfficeDto.TypeAccount == Rollen.BackendWorker)
                {
                    roleResult = await _userManager.AddToRoleAsync(appUser, Rollen.BackendWorker);
                }
                else
                {
                    roleResult = await _userManager.AddToRoleAsync(appUser, Rollen.FrontendWorker);
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

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPut("BlokkeerdVoertuig")] //methode voor het blokkeren van voertuigen zodat ze niet meer kunnen worden gebruikt bij verhuuringen
        public async Task<IActionResult> BlokkeerVoertuig(int voertuigId, string Opmerking)
        {
            try
            {
                var result = await _voertuigService.BlokkeerVoertuig(voertuigId, Opmerking);
                if (!result)
                {
                    return BadRequest(new { message = $"Geen voertuig gevonden met id {voertuigId}" });
                }
                return Ok(new { message = $"status van het voertuig met id {voertuigId} is veranderd naar {VoertuigStatussen.Geblokkeerd}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPut("DeblokkeerVoertuig")] // deblokkeerd een voertuig kan weer worden gebruikt
        public async Task<IActionResult> DeblokkeerVoertuig(int voertuigId)
        {
            try
            {
                var result = await _voertuigService.DeBlokkeerVoertuig(voertuigId);
                if (!result)
                {
                    return BadRequest(new { message = $"Geen voertuig gevonden met id {voertuigId} of het voertuig is momenteel niet geblokkeerd" });
                }
                return Ok(new { message = $"status van het voertuig met id {voertuigId} is veranderd naar {VoertuigStatussen.KlaarVoorGebruik}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpGet("GetAllSchadeMeldingen")] //methode voor opvragen schademeldingen om ze te kunnen verwerken
        public async Task<IActionResult> GetAllSchadeMeldingen()
        {
            try
            {
                var Schadeformulieren = await _voertuigService.GetAllIngediendeFormulieren();
                if (Schadeformulieren.Count == 0)
                {
                    return NotFound(new { message = "Geen schadeFormulieren gevonden die nog niet behandeld zijn" });
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPut("BehandelSchadeMelding")] //methode voor behandelen van een schademelding, hierbij moet de medewerken daadwerkelijk iets regelen voor het voertuig
        public async Task<IActionResult> BehandelSchadeMelding([FromBody] SchadeMeldingBehandelDto schadeMeldingBehandelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "ongeldige invoer" });
            }
            try
            {
                var succes = await _voertuigService.BehandelSchadeMelding(schadeMeldingBehandelDto.SchadeFormulierId, schadeMeldingBehandelDto.ReparatieOpmerking);
                if (!succes)
                {
                    return BadRequest(new { message = "Er is iets misgegaan bij het verwerken van de schade." });
                }
                return Ok(new { message = "Reparatie is succesvol afgehandeld en de voertuigstatus is aangepast." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPut("MeldSchade")] //methode voor het melden van schade voor backendworkers
        public async Task<IActionResult> MeldSchade([FromBody] MeldSchadeDto meldSchadeDto)
        {
            try
            {
                var result = await _reserveringService.MeldSchadeVanuitVoertuigId(meldSchadeDto.VoertuigId, meldSchadeDto.Schade, meldSchadeDto.SchadeFoto);
                if (!result)
                {
                    return BadRequest(new { message = "Er is iets misgegaan" });
                }
                return Ok(new { message = "Succes schade gemeld bij voertuig" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPost("AddVoertuig")] //methode voor toevoegen voertuigen
        public async Task<IActionResult> AddVoertuig([FromBody] NieuwVoertuigDto nieuwVoertuigDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Niet alle data is juist opgeleverd.", errors = ModelState });
            }
            try
            {
                var result = await _voertuigService.CreeerNieuwVoertuig(nieuwVoertuigDto);
                if (!result)
                {
                    return BadRequest(new { message = "Er is iets misgegaan bij het aanmaken van het voertuig." });
                }
                return Ok(new { message = $"Succesvol nieuw voertuig met kenteken: {nieuwVoertuigDto.Kenteken} toegevoegt." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpDelete("VerwijderVoertuig")] //methode voor verwijderen voertuigen
        public async Task<IActionResult> VerwijderVoertuig([FromBody] IdDto voertuigId)
        {
            try
            {
                var result = await _voertuigService.VerwijderVoertuig(voertuigId.Id);
                if (!result)
                {
                    return BadRequest(new { message = $"Er is iets misgegaan bij het verwijderen van het voertuig, waarschijn is er geen voertuig met id {voertuigId}." });
                }
                return Ok(new { message = $"Voertuig met id {voertuigId} is succesvol verwijderd" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPut("WijzigVoertuig")] //methode voor wijzigen van data van een voertuig
        public async Task<IActionResult> WijzigVoertuig([FromBody] WeizigVoertuigDto weizigVoertuigDto)
        {
            try
            {
                var result = await _voertuigService.WeizigVoertuig(weizigVoertuigDto);
                if (!result)
                {
                    return BadRequest(new { message = "Er is iets misgegaan bij het wijzigen van het voertuig." });
                }
                return Ok(new { message = $"Voertuig met id {weizigVoertuigDto.VoertuigId} is succesvol geweizigd." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpGet("GetAllNieuwWagenParkVerzoeken")] //methode vraagt alle verzoeken voor nieuwe wagenparkbeheerder op voor verwerking
        public async Task<IActionResult> GetAllNieuwWagenParkVerzoeken()
        {
            try
            {
                var verzoeken = await _wagenparkService.GetAllWagenparkVerzoekenAsync();
                if (verzoeken == null || verzoeken.Count == 0)
                {
                    return NotFound("Er zijn geen nieuwe verzoeken.");
                }

                return Ok(verzoeken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPut("AcceptVerzoek")] //accepteerd het verzoek en maak een nieuw wagenpark aan
        public async Task<IActionResult> AcceptVerzoek([FromBody] IdDto idDto)
        {
            try
            {
                var wagenPark = await _wagenparkService.AcceptNieuwWagenParkVerzoek(idDto.Id);
                return Ok("wagenpark succesvol aangemaakt.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = Rollen.BackendWorker)]
        [HttpPut("DeclineVerzoek")] //weiger het verzoek en stuur een email met de reden
        public async Task<IActionResult> DeclineVerzoek([FromBody] WeigerNieuwWagenParkVerzoekDto Dto)
        {
            try
            {
                var result = await _wagenparkService.WeigerNieuwWagenParkVerzoek(Dto);
                if (!result)
                {
                    return BadRequest("Er is iets misgegaan bij het afwijzen van het verzoek.");
                }
                return Ok("Verzoek succesvol afgewezen.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }   
}