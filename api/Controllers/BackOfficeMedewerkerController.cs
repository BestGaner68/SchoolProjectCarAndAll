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
    [Route("api/BackOfficeMedewerker")]
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

        [HttpPost("registerWagenparkBeheerder")]
        public async Task<IActionResult> RegisterWagenparkBeheerder([FromBody]RegisterWagenParkBeheerderDto registerWagenParkBeheerderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerWagenParkBeheerderDto.Username,
                    Email = registerWagenParkBeheerderDto.Email,
                    PhoneNumber = registerWagenParkBeheerderDto.PhoneNumber,
                    Voornaam = registerWagenParkBeheerderDto.Voornaam,
                    Achternaam =  registerWagenParkBeheerderDto.Achternaam,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerWagenParkBeheerderDto.Password);

                if (!createdUser.Succeeded)
                {
                    return StatusCode(500, createdUser.Errors);
                }

                WagenPark CreateWagenpark = WagenParkMapper.toWagenParkFromRegisterOfficeWorkerDto(registerWagenParkBeheerderDto);
                await _wagenparkService.CreateWagenparkAsync(CreateWagenpark, appUser.Id);

                var roleResult = await _userManager.AddToRoleAsync(appUser, "wagenparkBeheerder");

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