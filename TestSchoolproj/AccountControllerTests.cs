using System;
using System.Threading.Tasks;
using api.Controllers;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

using Microsoft.AspNetCore.Http;

namespace TestSchoolproj
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
        private readonly Mock<IWagenparkService> _mockWagenparkService;
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly Mock<IDoubleDataCheckerRepo> _mockDoubleDataCheckerRepo;
        private readonly Mock<IWagenParkUserListService> _mockWagenParkUserListService;
        private readonly Mock<IAbonnementService> _mockAbonnementService; // Added mock for IAbonnementService
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            // Initialize mocks
            _mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

            _mockTokenService = new Mock<ITokenService>();

            _mockSignInManager = new Mock<SignInManager<AppUser>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null);

            _mockWagenparkService = new Mock<IWagenparkService>();
            _mockRoleService = new Mock<IRoleService>();
            _mockDoubleDataCheckerRepo = new Mock<IDoubleDataCheckerRepo>();
            _mockWagenParkUserListService = new Mock<IWagenParkUserListService>();
            _mockAbonnementService = new Mock<IAbonnementService>(); // Initialize the mock

            // Pass all mocks to the controller
            _controller = new AccountController(
                _mockUserManager.Object,
                _mockTokenService.Object,
                _mockSignInManager.Object,
                _mockWagenparkService.Object,
                _mockRoleService.Object,
                _mockWagenParkUserListService.Object,
                _mockDoubleDataCheckerRepo.Object,
                _mockAbonnementService.Object); // Include the mock here
        }

        [Fact]
        public async Task RegisterParticulier_ShouldReturnOk_WhenUserIsCreatedSuccessfully()
        {
            // Arrange: Voorbereiden van een geldige input en mock-oproepen
            var registerDto = new RegisterDto
            {
                Username = "JanJansen",
                Email = "jan.jansen@example.com",
                Password = "VeiligWachtwoord123!"
            };

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            // De gebruiker wordt succesvol aangemaakt
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Rol wordt toegevoegd
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "particuliereKlant"))
                .ReturnsAsync(IdentityResult.Success);

            // Een token wordt succesvol gegenereerd
            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>()))
                .Returns("test-token");

            // Een standaard abonnement toekennen
            _mockAbonnementService.Setup(x => x.GeefStandaardAbonnement(It.IsAny<AppUser>()))
                .ReturnsAsync(true);

            // Act: Roept de RegisterParticulier-methode aan
            var result = await _controller.RegisterParticulier(registerDto);

            // Assert: Controleert of het resultaat een OkObjectResult is met de juiste gegevens
            var okResult = Assert.IsType<OkObjectResult>(result);
            var newUserDto = Assert.IsType<NewUserDto>(okResult.Value);

            Assert.Equal(registerDto.Username, newUserDto.Username);
            Assert.Equal(registerDto.Email, newUserDto.Email);
            Assert.Equal("test-token", newUserDto.Token);
        }

        [Fact]
        public async Task RegisterParticulier_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange: Er worden hier ongeldige gegevens gebruikt
            _controller.ModelState.AddModelError("Username", "Required");

            var registerDto = new RegisterDto
            {
                Username = "",
                Email = "jan.jansen@example.com",
                Password = "VeiligWachtwoord123!"
            };

            // Act: Roept de RegisterParticulier-methode aan
            var result = await _controller.RegisterParticulier(registerDto);

            // Assert: Controleert of het resultaat een BadRequestObjectResult is
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterParticulier_ShouldReturnServerError_WhenUserCreationFails()
        {
            // Arrange: De gebruiker wordt hier niet succesvol aangemaakt
            var registerDto = new RegisterDto
            {
                Username = "JanJansen",
                Email = "jan.jansen@example.com",
                Password = "VeiligWachtwoord123!"
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed." }));

            // Act: Roept de RegisterParticulier-methode aan
            var result = await _controller.RegisterParticulier(registerDto);

            // Assert: Controleert of het resultaat een ObjectResult is met een statuscode 500
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }
    }
}