using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.Account;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWagenparkService _wagenparkService;
        private readonly IWagenParkUserListService _wagenparkUserListService;
        private readonly IRoleService _roleService;
        private readonly IDoubleDataCheckerRepo _doubleDataCheckerRepo;
        private readonly IAbonnementService _abonnementService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
        SignInManager<AppUser> signInManager, IWagenparkService wagenparkService, IRoleService roleService,
        IWagenParkUserListService wagenParkUserListService,
        IDoubleDataCheckerRepo doubleDataCheckerRepo, IAbonnementService abonnementService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _wagenparkService = wagenparkService;
            _roleService = roleService;
            _doubleDataCheckerRepo = doubleDataCheckerRepo;
            _wagenparkUserListService = wagenParkUserListService;
            _abonnementService = abonnementService; 
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            };

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if(user == null) return Unauthorized("Invalid Username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if(!result.Succeeded)
            {
                return Unauthorized("Username not found or password incorrect");
            };

            return Ok(
                new NewUserDto{
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("registerParticulier")]
        public async Task<IActionResult> RegisterParticulier ([FromBody]RegisterDto registerDto){
            try 
            {
                if(!ModelState.IsValid)
                    return BadRequest (ModelState);

                var AppUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    Voornaam = registerDto.Voornaam,
                    Achternaam = registerDto.Achternaam,
                };

                var createdUser = await _userManager.CreateAsync(AppUser, registerDto.Password);

                if(createdUser.Succeeded){
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "particuliereKlant");
                    if (roleResult.Succeeded){
                        var succes = await _abonnementService.GeefStandaardAbonnement(AppUser);
                        if (!succes)
                        {
                            return BadRequest (new {message = "er is iets misgegaan"});
                        }
                        return Ok(
                            new NewUserDto
                            {
                                Username = AppUser.UserName,
                                Email = AppUser.Email,
                                Token = _tokenService.CreateToken(AppUser)
                            }
                        );
                    }
                    else{
                        return StatusCode (500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }


            }
            catch(Exception e)
            {
                return StatusCode(500, e);

            }

        }

        [HttpPost("registerZakelijk")]
        public async Task<IActionResult> RegisterZakelijk([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var TempWagenPark = await _wagenparkUserListService.GetWagenParkByAppUserEmail(registerDto.Email);
                if (TempWagenPark== null)
                {
                    return NotFound(new { message = "E-mailadres is niet geregistreerd in de WagenParkUserList tabel." });
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    Voornaam = registerDto.Voornaam,
                    Achternaam = registerDto.Achternaam,
                };

                var createdUserResult = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (!createdUserResult.Succeeded)
                    return StatusCode(500, createdUserResult.Errors);

                var roleResult = await _userManager.AddToRoleAsync(appUser, Rollen.BedrijfsKlant);
                if (!roleResult.Succeeded)
                    return StatusCode(500, roleResult.Errors);

                var linked = await _wagenparkUserListService.PrimeUserInWagenParkUserList(appUser.Email, WagenParkUserListStatussen.Toegevoegt, appUser.Email, TempWagenPark.WagenParkId);
                if (!linked)
                {
                    return BadRequest(new { message = "gebruiker niet toegevoegt aan wagenpark" });
                }

                return Ok(new NewUserDto
                {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Token = _tokenService.CreateToken(appUser)
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("getUserData")]
        [Authorize] 
        public async Task<IActionResult> GetUserData()
        {
            try
            {    
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized(new {message = "JWT Token is niet meer in gebruik"});
                }

                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized("No userID found in token.");
                }
                
                var appUser = await _userManager.FindByIdAsync(AppUserId);

                if (appUser == null)
                {
                    return NotFound("User not found");
                }
            
                var dto = new UserDataDto
                {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    PhoneNumber = appUser.PhoneNumber,
                    Voornaam = appUser.Voornaam,
                    Achternaam = appUser.Achternaam,
                    role = await _roleService.getUserRole(appUser.Id),
                };

                return Ok(dto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("updateUserData")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData([FromBody] UpdateUserDto updateDto)
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized("No userID found in token.");
                }

                var appUser = await _userManager.FindByIdAsync(AppUserId);
                if (appUser == null)
                {
                    return NotFound("User not found");
                }

                if (appUser.UserName != updateDto.Username && await _doubleDataCheckerRepo.UsernameTaken(updateDto.Username))
                {
                    return BadRequest("Username is already taken.");
                }

                if (appUser.Email != updateDto.Email && await _doubleDataCheckerRepo.EmailTaken(updateDto.Email))
                {
                    return BadRequest("Email is already taken.");
                }

                if (appUser.PhoneNumber != updateDto.PhoneNumber && await _doubleDataCheckerRepo.PhoneNumberTaken(updateDto.PhoneNumber))
                {
                    return BadRequest("Phone number is already taken.");
                }

                appUser.UserName = updateDto.Username;
                appUser.Email = updateDto.Email;
                appUser.PhoneNumber = updateDto.PhoneNumber;

                var result = await _userManager.UpdateAsync(appUser);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("User updated successfully.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized("No userID found in token.");
                }
                var appUser = await _userManager.FindByIdAsync(AppUserId);
                if (appUser == null)
                {
                    return NotFound("User not found.");
                }
                var passwordCheckResult = await _userManager.CheckPasswordAsync(appUser, changePasswordDto.OldPassword);
                if (!passwordCheckResult)
                {
                    return BadRequest("The old password is incorrect.");
                }
                var result = await _userManager.ChangePasswordAsync(appUser, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest($"Error changing password: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                return Ok("Password changed successfully.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("NieuwWagenParkVerzoek")]
        public async Task<IActionResult> NieuwWagenParkVerzoek(NieuwWagenParkVerzoekDto wagenParkVerzoekDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _wagenparkService.NieuwWagenParkVerzoek(wagenParkVerzoekDto);
            return Ok("Verzoek is verzonden");
        }
    }
}