using System.Threading.Tasks;
using api.Controllers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestApi
{
    public class VerhuurVerzoekControllerTests
    {
        private readonly Mock<IVerhuurVerzoekService> _mockVerhuurVerzoekService;
        private readonly Mock<IVoertuigService> _mockVoertuigService;
        private readonly VerhuurVerzoekController _controller;

        public VerhuurVerzoekControllerTests()
        {
            // Mock objecten voor de services die de controller gebruikt
            _mockVerhuurVerzoekService = new Mock<IVerhuurVerzoekService>();
            _mockVoertuigService = new Mock<IVoertuigService>();

            // Controller initialiseren met de mock services
            _controller = new VerhuurVerzoekController(_mockVerhuurVerzoekService.Object, _mockVoertuigService.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenVerhuurVerzoekDoesNotExist()
        {
            // Arrange: Er wordt geen VerhuurVerzoek gevonden met het opgegeven ID
            int verzoekId = 99;
            _mockVerhuurVerzoekService.Setup(x => x.GetByIdAsync(verzoekId)).ReturnsAsync((VerhuurVerzoek)null);

            // Act: Roept de GetById-methode van de controller aan
            var result = await _controller.GetById(verzoekId);

            // Assert: Controleert of het resultaat een NotFoundResult is
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenVerhuurVerzoekExists()
        {
            // Arrange: Er wordt een VerhuurVerzoek gevonden met het opgegeven ID
            int verzoekId = 1;
            var verzoek = new VerhuurVerzoek { VerhuurVerzoekId = verzoekId };
            _mockVerhuurVerzoekService.Setup(x => x.GetByIdAsync(verzoekId)).ReturnsAsync(verzoek);

            // Act: Roept de GetById-methode van de controller aan
            var result = await _controller.GetById(verzoekId);

            // Assert: Controleert of het resultaat een OkObjectResult is en of de waarde overeenkomt met het verzoek
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(verzoek, okResult.Value);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange: Er wordt ongeldige ModelState gegeven
            _controller.ModelState.AddModelError("Test", "Invalid model");

            // Act: Roept de Create-methode van de controller aan met een null-model
            var result = await _controller.Create(null);

            // Assert: Controleert of het resultaat een BadRequestObjectResult is
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

