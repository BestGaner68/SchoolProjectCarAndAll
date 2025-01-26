using System.Threading.Tasks;
using api.Controllers;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestSchoolproj
{
    public class FrontOfficeMedewerkerControllerTests
    {
        private readonly Mock<IReserveringService> _mockReserveringService;
        private readonly FrontOfficeMedewerkerController _controller;

        public FrontOfficeMedewerkerControllerTests()
        {

            // Mock object voor de reserveringsservice
            _mockReserveringService = new Mock<IReserveringService>();

            // Controller initialiseren met de mock service
            _controller = new FrontOfficeMedewerkerController(_mockReserveringService.Object);
        }

        [Fact]
        public async Task GeefUit_ShouldReturnBadRequest_WhenServiceReturnsFalse()
        {
            // Arrange: Zorgt ervoor dat de GeefUit-methode van de service false retourneert
            var reserveringDto = new IdDto { Id = 1 };
            _mockReserveringService.Setup(x => x.GeefUit(reserveringDto.Id)).ReturnsAsync(false);

            // Act: Roept de GeefUit-methode van de controller aan
            var result = await _controller.GeefUit(reserveringDto);

            // Assert: Controleert of het resultaat een BadRequestObjectResult is en of de bericht overeenkomt
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Er is iets mis gegaan", badRequestResult.Value);
        }

        [Fact]
        public async Task GeefUit_ShouldReturnOk_WhenServiceReturnsTrue()
        {
            // Arrange: Zorgt ervoor dat de GeefUit-methode van de service true retourneert
            var reserveringDto = new IdDto { Id = 2 };
            _mockReserveringService.Setup(x => x.GeefUit(reserveringDto.Id)).ReturnsAsync(true);

            // Act: Roept de GeefUit-methode van de controller aan
            var result = await _controller.GeefUit(reserveringDto);

            // Assert: Controleert of het resultaat een OkObjectResult is en of de bericht overeenkomt
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Reservering is uitgegeven", okResult.Value);
        }

        [Fact]
        public async Task NeemIn_ShouldReturnBadRequest_WhenSchadeIsTrueAndModelStateIsInvalid()
        {
            // Arrange: Voegt een fout toe aan de ModelState om een ongeldige invoer te simuleren
            var innameDto = new InnameDto { IsSchade = true, Schade = null };
            _controller.ModelState.AddModelError("Schade", "Required");

            // Act: Roept de NeemIn-methode van de controller aan
            var result = await _controller.NeemIn(innameDto);

            // Assert: Controleert of het resultaat een BadRequestObjectResult is en of de bericht overeenkomt
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Vul alle verplichte velden in.", badRequestResult.Value);
        }
    }
}

