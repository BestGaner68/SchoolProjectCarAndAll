using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Mapper;
using api.Migrations;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    
    public class BackOfficeMedewerkerController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWagenparkService _wagenparkService;
        private readonly IVoertuigHelper _voertuigHelper;
        public BackOfficeMedewerkerController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IWagenparkService wagenparkService, IVoertuigHelper voertuigHelper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _wagenparkService = wagenparkService;
            _voertuigHelper = voertuigHelper;
        }

        [HttpPost("manualRegister")]
        public async Task<IActionResult> RegisterBackEnd([FromBody] RegisterOfficeWorkerDto registerOfficeDto)
        {     
        try
            {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validRoles = new[] { "BackendWorker", "FrontendWorker", "WagenparkBeheerder" };
            if (!validRoles.Contains(registerOfficeDto.TypeAccount))
            {
                return BadRequest("Verkeerde Rol, Mogelijkheden: BackendWorker, frontendWorker en Wagenparkbeheerder.");
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
            switch (registerOfficeDto.TypeAccount)
            {
                case "BackendWorker":
                    roleResult = await _userManager.AddToRoleAsync(appUser, "backendWorker");
                    break;

                case "FrontendWorker":
                    roleResult = await _userManager.AddToRoleAsync(appUser, "frontendWorker");
                    break;

                case "WagenparkBeheerder":
                    WagenPark CreateWagenpark = WagenParkMapper.toWagenParkFromRegisterOfficeWorkerDto(registerOfficeDto);
                    await _wagenparkService.CreateWagenparkAsync(CreateWagenpark, registerOfficeDto.Username);
                    roleResult = await _userManager.AddToRoleAsync(appUser, "wagenparkBeheerder");
                    break;

                default:
                    return BadRequest(); //zou nooit moeten triggeren
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


        [HttpPut]
        public async Task<IActionResult> ChangeVoertuigStatus (int voertuigId, string status){
            var result = await _voertuigHelper.ChangeStatusVoertuig(voertuigId, status);
            if (!result){
                return BadRequest($"Geen voertuig gevonden met id {voertuigId}");
            }
            return Ok($"status van het voertuig met id {voertuigId} is veranderd naar {status}");
        }  
    }   
}