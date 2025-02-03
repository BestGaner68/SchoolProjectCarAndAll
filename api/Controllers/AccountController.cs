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
        private readonly IDoubleDataCheckerRepo _doubleDataCheckerRepo;
        private readonly IAbonnementService _abonnementService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
        SignInManager<AppUser> signInManager, IWagenparkService wagenparkService,
        IWagenParkUserListService wagenParkUserListService,
        IDoubleDataCheckerRepo doubleDataCheckerRepo, IAbonnementService abonnementService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _wagenparkService = wagenparkService;
            _doubleDataCheckerRepo = doubleDataCheckerRepo;
            _wagenparkUserListService = wagenParkUserListService;
            _abonnementService = abonnementService; 
        }


        /// <summary>
        /// Login methode voor het inloggen en geven van jwt token
        /// </summary>
        /// <param name="loginDto">gegevens voor het inloggen Username, password</param>
        /// <returns>de Jwt token van de user, username en email</returns>
        [HttpPost("Login")] 
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid Username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username not found or password incorrect");
            }

            return Ok(
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        /// <summary>
        /// Register methode voor particuliere klanten
        /// </summary>
        /// <param name="registerDto">register gegevens zoals naam, username, password etc.</param>
        /// <returns>jwt token, username en email</returns>
        [HttpPost("registerParticulier")] 
        public async Task<IActionResult> RegisterParticulier([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var AppUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    Voornaam = registerDto.Voornaam,
                    Achternaam = registerDto.Achternaam,
                };

                var createdUser = await _userManager.CreateAsync(AppUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "particuliereKlant");
                    if (roleResult.Succeeded)
                    {
                        var succes = await _abonnementService.GeefStandaardAbonnement(AppUser);
                        if (!succes)
                        {
                            return BadRequest(new { message = "er is iets misgegaan" });
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
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Register speciefiek voor het aanmaken van een account binnen een wagenpark, de methode checked of de user is uitgenodigd oftwel in de db staat met zijn email
        /// </summary>
        /// <param name="registerDto">register gegevens naam, username etc.</param>
        /// <returns>jwt token, username, email</returns>
        [HttpPost("registerZakelijk")]
        public async Task<IActionResult> RegisterZakelijk([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);



                var TempWagenPark = await _wagenparkUserListService.GetWagenParkByAppUserEmail(registerDto.Email);
                if (TempWagenPark == null)
                {
                    return NotFound(new { message = "E-mailadres is niet geregistreerd in de WagenParkUserList tabel." });
                }
                var StopHere = await _wagenparkService.IsVerwijderdeGebruiker(registerDto.Email, TempWagenPark.WagenParkId);
                if (StopHere)
                {
                    return BadRequest("U bent permanent verwijderd van dit wagenpark joinen.");
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

                var linked = await _wagenparkUserListService.PrimeUserInWagenParkUserList(appUser.Id, WagenParkUserListStatussen.Toegevoegt, appUser.Email, TempWagenPark.WagenParkId);
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

        /// <summary>
        /// Methode returned gegevens van de gebruiker gebaseerd op de jwt token voor gebruik in de frontend
        /// </summary>
        /// <returns>belangrijke gegevens van de gebruiker voornamelijk zijn rol, zie methode</returns>
        [HttpGet("getUserData")] //methode voor userdata wordt gebruikt in de frontend voor routing
        [Authorize]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(AppUserId))
                {
                    return Unauthorized(new { message = "JWT Token is niet meer in gebruik" });
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
                    role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault(),
                };

                return Ok(dto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// methode voor het veranderen van gegevens op gebruikers profiel
        /// </summary>
        /// <param name="updateDto">de gegevens die zij willen veranderen username, email en phonenumber </param>
        /// <returns>niets, maar past aan in de db als de gegevens kloppen</returns>
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

        /// <summary>
        /// specifieke methode voor aanpassen van user wachtwoord
        /// </summary>
        /// <param name="changePasswordDto">verwacht current wachtwoord en het in te veranderen wachtwoord</param>
        /// <returns>niets</returns>
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

        /// <summary>
        /// methode voor het aanmaken van een verzoek om een wagenparkeigen te worden op onze website, als alles klopt kan de backendmedewerker het goedkeuren
        /// </summary>
        /// <param name="wagenParkVerzoekDto">Belangijke gegevens bij het aanmaken zoals gebruikers info voor account en gegevens voor het wagenpark zelf kvk nummer, bedrijfsnaam etc.</param>
        /// <returns>niets, zet een request in de db</returns>
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