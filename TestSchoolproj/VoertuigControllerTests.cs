using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestSchoolproj
{
    public class VoertuigControllerTests
    {
        private readonly Mock<IVoertuigService> _mockVoertuigService;
        private readonly VoertuigController _controller;

        public VoertuigControllerTests()
        {

            // Mock object voor de voertuigservice
            _mockVoertuigService = new Mock<IVoertuigService>();
            // Controller initialiseren met de mock service
            _controller = new VoertuigController(_mockVoertuigService.Object);
        }

        [Fact]
        public async Task GetAllVoertuigen_ShouldReturnOk_WhenVoertuigenExist()
        {
            // Arrange: Een lijst van voertuigen die wordt geretourneerd door de service
            var mockVoertuigen = new List<Voertuig> { new Voertuig { Merk = "Toyota", Soort = "Auto" } };
            _mockVoertuigService.Setup(x => x.GetAllVoertuigen()).Returns(Task.FromResult(mockVoertuigen));

            // Act: Roept de GetAllVoertuigen-methode van de controller aan
            var result = await _controller.GetAllVoertuigen();

            // Assert: Controleert of het resultaat overeenkomt
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Voertuig>>(okResult.Value);
            Assert.Equal(mockVoertuigen.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetAllVoertuigen_ShouldReturnEmptyList_WhenNoVoertuigenExist()
        {
            // Arrange: Dit simuleert een lege lijst van voertuigen die wordt geretourneerd door de service
            _mockVoertuigService.Setup(x => x.GetAllVoertuigen()).Returns(Task.FromResult(new List<Voertuig>()));

            // Act: Roept de GetAllVoertuigen-methode van de controller aan
            var result = await _controller.GetAllVoertuigen();

            // Assert: Controleert of het resultaat overeenkomt
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Voertuig>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task GetVoertuigenByMerk_ShouldReturnOk_WhenVoertuigenFound()
        {
            // Arrange: Dit simuleert een lijst van voertuigen die wordt geretourneerd door de service voor een specifiek merk
            var voertuigMerk = "Toyota";
            var mockVoertuigen = new List<Voertuig> { new Voertuig { Merk = "Toyota", Soort = "Auto" } };
            _mockVoertuigService.Setup(x => x.GetVoertuigenByMerk(voertuigMerk)).Returns(Task.FromResult(mockVoertuigen));

            // Act: Roep de GetVoertuigenByMerk-methode van de controller aan
            var result = await _controller.GetVoertuigenByMerk(voertuigMerk);

            // Assert: Controleert of het resultaat overeenkomt
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Voertuig>>(okResult.Value);
            Assert.Equal(mockVoertuigen.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetVoertuigenBySoort_ShouldReturnOk_WhenVoertuigenFound()
        {
            // Arrange: Dit simuleert een lijst van voertuigen die wordt geretourneerd door de service voor een specifieke soort
            var voertuigSoort = "Auto";
            var mockVoertuigen = new List<Voertuig> { new Voertuig { Merk = "Toyota", Soort = "Auto" } };
            _mockVoertuigService.Setup(x => x.GetVoertuigenBySoort(voertuigSoort)).Returns(Task.FromResult(mockVoertuigen));

            // Act: Roept de GetVoertuigenBySoort-methode van de controller aan
            var result = await _controller.GetVoertuigenBySoort(voertuigSoort);

            // Assert: Controleert of het resultaat overeenkomt
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Voertuig>>(okResult.Value);
            Assert.Equal(mockVoertuigen.Count, returnValue.Count);
        }
    }
}

