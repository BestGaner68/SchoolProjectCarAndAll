using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Migrations;
using api.Models;
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
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IWagenparkService wagenparkService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _wagenparkService = wagenparkService;
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
                if (bedrijf == null){
                    return BadRequest(new {message = $"Geen wagenpark gevonden met emailstring: {registerDto.Email}"});
                }
                
                var AppUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(AppUser, registerDto.Password);

                if(createdUser.Succeeded){
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "bedrijfKlant");
                    if (roleResult.Succeeded){
                        var result = await _wagenparkService.CreateWagenParkVerzoek(AppUser.Id, bedrijf.WagenParkId);
                        if (result)
                        {
                            return Ok(
                            new NewUserDto
                            {
                                Username = AppUser.UserName,
                                Email = AppUser.Email,
                                //Token = _tokenService.CreateToken(AppUser)
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

    }
}