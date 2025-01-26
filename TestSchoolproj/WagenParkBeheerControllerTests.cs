using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Controllers;
using api.Dtos;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestSchoolproj
{
    public class WagenParkBeheerControllerTests
    {
        private readonly Mock<IWagenParkUserListService> _mockWagenParkUserListService;
        private readonly Mock<IWagenparkService> _mockWagenparkService;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly WagenParkBeheerController _controller;

        public WagenParkBeheerControllerTests()
        {

            // Mock services initialiseren
            _mockWagenParkUserListService = new Mock<IWagenParkUserListService>();
            _mockWagenparkService = new Mock<IWagenparkService>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

            // Mock gebruiker instellen
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUserId")
            }, "mock"));

            // Controller instellen met mock services en gebruiker
            _controller = new WagenParkBeheerController(
                _mockWagenParkUserListService.Object,
                _mockUserManager.Object,
                _mockWagenparkService.Object,
                _mockWagenParkUserListService.Object
            )
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };
        }

        [Fact]
        public async Task GetAllUserInWagenPark_ShouldReturnOk_WhenUsersExist()
        {
            // Arrange: Simuleert een lijst van gebruikers die beschikbaar zijn in het wagenpark
            var users = new List<AppUser> { new AppUser { Id = "1", UserName = "TestUser" } };
            _mockWagenParkUserListService.Setup(x => x.GetAllUsersInWagenPark("TestUserId"))
                .ReturnsAsync(users);

            // Act: Roept de GetAllUserInWagenPark-methode van de controller aan
            var result = await _controller.GetAllUserInWagenPark();

            // Assert: Controleert of het resultaat een OkObjectResult is en dat het niet null is
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task NodigGebruikerUitVoorWagenpark_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange: Simuleert een succesvolle uitnodiging voor een gebruiker
            var nodigUitDto = new NodigUitDto { Email = "test@example.com" };
            _mockWagenParkUserListService.Setup(x => x.StuurInvite(nodigUitDto.Email, "TestUserId"))
                .ReturnsAsync(true);

            // Act: Roept de NodigGebruikerUitVoorWagenpark-methode van de controller aan
            var result = await _controller.NodigGebruikerUitVoorWagenpark(nodigUitDto);

            // Assert: Controleert of het resultaat een NoContentResult is
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAllUserInWagenPark_ShouldReturnBadRequest_WhenNoUsersExist()
        {
            // Arrange: Simuleert een lege lijst van gebruikers in het wagenpark
            _mockWagenParkUserListService.Setup(x => x.GetAllUsersInWagenPark("TestUserId"))
                .ReturnsAsync(new List<AppUser>());

            // Act: Roept de GetAllUserInWagenPark-methode van de controller aan
            var result = await _controller.GetAllUserInWagenPark();

            // Assert: Controleert of het resultaat een BadRequestObjectResult is en dat het niet null is
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}


