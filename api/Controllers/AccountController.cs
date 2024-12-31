using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Migrations;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWagenparkService _wagenparkService;
        private readonly IRoleService _roleService;
        private readonly IDoubleDataCheckerRepo _doubleDataCheckerRepo;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
        SignInManager<AppUser> signInManager, IWagenparkService wagenparkService, IRoleService roleService,
        IDoubleDataCheckerRepo doubleDataCheckerRepo)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _wagenparkService = wagenparkService;
            _roleService = roleService;
            _doubleDataCheckerRepo = doubleDataCheckerRepo;
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
        public async Task<IActionResult> RegisterZakelijk ([FromBody]RegisterDto registerDto){
            try 
            {
                if(!ModelState.IsValid)
                    return BadRequest (ModelState);

                var bedrijf = await _wagenparkService.GetWagenParkByEmail(registerDto.Email);    
                
                var AppUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(AppUser, registerDto.Password);

                if(createdUser.Succeeded){
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "pending");
                    if (roleResult.Succeeded){
                        var result = await _wagenparkService.CreateWagenParkVerzoek(AppUser.Id, bedrijf.WagenParkId);
                        if (result)
                        {
                            return Ok(
                            new NewUserDto
                            {
                                Username = AppUser.UserName,
                                Email = AppUser.Email,
                                Token = _tokenService.CreateToken(AppUser)
                            }
                            );
                        }
                        else
                        {
                            return BadRequest(new { message = "Error linking user to Wagenpark." });
                        }
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
            catch(Exception ex)
            {
                var errorResponse = new 
            {
                Message = ex.Message,  // Include the error message
                StackTrace = ex.StackTrace // Include the stack trace if needed
            };

            // Return a generic error response
            return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("getUserData")]
        [Authorize]  // Zorg ervoor dat alleen geauthenticeerde gebruikers deze route kunnen aanroepen
        public async Task<IActionResult> GetUserData()
        {
            try
            {    
                
                var username = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                Console.WriteLine("Username from token: " + username);

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("No username found in token.");
                }

                
                var appUser = await _userManager.FindByNameAsync(username);

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
                var username = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("No username found in token.");
                }

                var appUser = await _userManager.FindByNameAsync(username);
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
    }
}