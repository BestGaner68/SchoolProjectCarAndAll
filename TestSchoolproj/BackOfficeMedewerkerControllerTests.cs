using System.Threading.Tasks;
using api.Controllers;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestSchoolproj
{
    public class BackOfficeMedewerkerControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IWagenparkService> _mockWagenparkService;
        private readonly Mock<IVoertuigService> _mockVoertuigService;
        private readonly Mock<IReserveringService> _mockReserveringService;
        private readonly BackOfficeMedewerkerController _controller;

        public BackOfficeMedewerkerControllerTests()
        {
            // Mocks instellen voor de benodigde services en UserManager
            _mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

            _mockTokenService = new Mock<ITokenService>();
            _mockWagenparkService = new Mock<IWagenparkService>();
            _mockVoertuigService = new Mock<IVoertuigService>();
            _mockReserveringService = new Mock<IReserveringService>();

            // Initialiseert de controller met de gemockte services
            _controller = new BackOfficeMedewerkerController(
                _mockUserManager.Object,
                _mockTokenService.Object,
                _mockWagenparkService.Object,
                _mockVoertuigService.Object,
                _mockReserveringService.Object);
        }

        [Fact]
        public async Task RegisterBackendAndFrontend_ShouldReturnBadRequest_WhenRoleIsInvalid()
        {
            // Arrange: Stelt een DTO in met een ongeldige rol
            var registerDto = new RegisterBackOrFrontEndWorkerDto
            {
                Username = "JohnDoe",
                Email = "john.doe@example.com",
                Password = "SecurePassword123!",
                TypeAccount = "InvalidRole"
            };

            // Act: Roept de methode aan met de ongeldige rol
            var result = await _controller.RegisterBackendAndFrontend(registerDto);

            // Assert: Controleert of het resultaat een BadRequestObjectResult is en of het juiste bericht wordt geretourneerd
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Verkeerde Rol, Mogelijkheden: BackendWorker, FrontendWorker.", badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterBackendAndFrontend_ShouldReturnServerError_WhenUserCreationFails()
        {
            // Arrange: Stelt een DTO in en het aanmaken van een gebruiker wordt mislukt
            var registerDto = new RegisterBackOrFrontEndWorkerDto
            {
                Username = "JohnDoe",
                Email = "john.doe@example.com",
                Password = "SecurePassword123!",
                TypeAccount = "BackendWorker"
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed." }));

            // Act: Roept de methode aan
            var result = await _controller.RegisterBackendAndFrontend(registerDto);

            // Assert: Controleert of het resultaat een ObjectResult met statuscode 500 is
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }

        [Fact]
        public async Task RegisterBackendAndFrontend_ShouldReturnServerError_WhenRoleAssignmentFails()
        {
            // Arrange: Stelt een DTO in en het toewijzen van een rol wordt mislukt
            var registerDto = new RegisterBackOrFrontEndWorkerDto
            {
                Username = "JohnDoe",
                Email = "john.doe@example.com",
                Password = "SecurePassword123!",
                TypeAccount = "BackendWorker"
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "backendWorker"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed." }));

            // Act: Roept de methode aan
            var result = await _controller.RegisterBackendAndFrontend(registerDto);

            // Assert: Controleert of het resultaat een ObjectResult met statuscode 500 is
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }
    }
}

