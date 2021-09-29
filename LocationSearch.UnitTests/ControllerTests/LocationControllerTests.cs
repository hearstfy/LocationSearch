using System;
using System.Collections.Generic;
using LocationSearch.Api.Controllers;
using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Services;
using Moq;
using Xunit;
using FluentAssertions;
namespace LocationSearch.UnitTests
{
    public class UnitTesLocationControllerTests
    {
        private readonly Random rand = new();

        [Fact]
        public void GetLocations_WithValidRequestBody_ReturnsLocationList()
        {
            var locationServiceStub = new Mock<ILocationService>();
            var controller = new LocationsController(locationServiceStub.Object);
            var requestDto = new FindLocationsRequestDto() { Latitude = 53.63764880, Longitude = 4.73995440, MaxDistance = 100, MaxResults = 100 };

            var result = controller.GetLocations(requestDto);

            Assert.IsAssignableFrom<IEnumerable<Location>>(result).Should().NotBeNull();
        }
    }
}
