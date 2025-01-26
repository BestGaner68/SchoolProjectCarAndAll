using System;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Controllers;
using api.Dtos;
using api.Dtos.Verhuur;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestSchoolproj
{
    public class AbonnementControllerTests
    {
        private readonly Mock<IAbonnementService> _mockAbonnementService;
        private readonly Mock<IWagenparkService> _mockWagenparkService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly AbonnementController _controller;

        public AbonnementControllerTests()
        {
            // Initialiseert de mocks
            _mockAbonnementService = new Mock<IAbonnementService>();
            _mockWagenparkService = new Mock<IWagenparkService>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            // Stelt de mock in voor de HttpContext
            var context = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, "user-id")
    }));
            context.User = userClaims;
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            // Geeft alle mocks door aan de controller
            _controller = new AbonnementController(
                _mockAbonnementService.Object,
                _mockWagenparkService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                }
            };
        }

        [Fact]
        public async Task WijzigAbonnementUser_ShouldReturnOk_WhenAbonnementIsChangedSuccessfully()
        {
            // Arrange: Bereidt de invoer voor en stel de mock in
            var nieuwAbonnementId = new IdDto { Id = 2 };
            _mockAbonnementService.Setup(x => x.WijzigAbonnementUser("user-id", nieuwAbonnementId.Id))
                .ReturnsAsync(true);

            // Act: Roept de methode aan
            var result = await _controller.WijzigAbonnementUser(nieuwAbonnementId);

            // Assert: Controleert het resultaat
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Abonnement succesvol gewijzigd.", okResult.Value);
        }

        [Fact]
        public async Task WijzigAbonnementUser_ShouldReturnBadRequest_WhenAbonnementCannotBeChanged()
        {
            // Arrange: Bereidt de invoer voor en stel de mock in
            var nieuwAbonnementId = new IdDto { Id = 2 };
            _mockAbonnementService.Setup(x => x.WijzigAbonnementUser("user-id", nieuwAbonnementId.Id))
                .ReturnsAsync(false);

            // Act: Roept de methode aan
            var result = await _controller.WijzigAbonnementUser(nieuwAbonnementId);

            // Assert: Controleert het resultaat
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Abonnement kan niet gewijzigd worden.", badRequestResult.Value);
        }

        [Fact]
        public async Task WijzigAbonnementWagenpark_ShouldReturnOk_WhenAbonnementIsChangedForWagenpark()
        {
            // Arrange: Bereidt de invoer voor en stel de mock in
            var abonnementWijzigDto = new AbonnementWeizigDto
            {
                WagenparkId = 1,
                NieuwAbonnementId = 2
            };
            _mockAbonnementService.Setup(x => x.WijzigAbonnementWagenpark(abonnementWijzigDto.WagenparkId, abonnementWijzigDto.NieuwAbonnementId))
                .ReturnsAsync(true);

            // Act: Roept de methode aan
            var result = await _controller.WijzigAbonnementWagenpark(abonnementWijzigDto);

            // Assert: Controleert het resultaat
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Wagenpark abonnement succesvol gewijzigd.", okResult.Value);
        }

        [Fact]
        public async Task WijzigAbonnementWagenpark_ShouldReturnBadRequest_WhenAbonnementCannotBeChangedForWagenpark()
        {
            // Arrange: Bereidt de invoer voor en stel de mock in
            var abonnementWijzigDto = new AbonnementWeizigDto
            {
                WagenparkId = 1,
                NieuwAbonnementId = 2
            };
            _mockAbonnementService.Setup(x => x.WijzigAbonnementWagenpark(abonnementWijzigDto.WagenparkId, abonnementWijzigDto.NieuwAbonnementId))
                .ReturnsAsync(false);

            // Act: Roept de methode aan
            var result = await _controller.WijzigAbonnementWagenpark(abonnementWijzigDto);

            // Assert: Controleert het resultaat
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Abonnement kan niet gewijzigd worden.", badRequestResult.Value);
        }
    }
}
