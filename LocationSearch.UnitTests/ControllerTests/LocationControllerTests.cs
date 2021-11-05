using LocationSearch.Api.Controllers;
using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Services;
using FakeItEasy;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LocationSearch.UnitTests
{
    public class UnitTesLocationControllerTests
    {

        [Fact]
        public void GetLocations_WithValidRequestBody_Returns_Correct_Number_Of_Locations()
        {
            //Arrange
            var locationService = A.Fake<ILocationService>();
            var requestDto = new FindLocationsRequestDto() { Latitude = 53.63764880, Longitude = 4.73995440, MaxDistance = 100, MaxResults = 100 };
            var fakeResult = A.CollectionOfDummy<Location>((int)requestDto.MaxResults).AsEnumerable();
            
            A.CallTo(() => locationService.FindNearestLocations((double) requestDto.Latitude, (double) requestDto.Longitude, (double) requestDto.MaxDistance, requestDto.MaxResults ?? null))
                .Returns(Task.FromResult(fakeResult));
            
            var controller = new LocationsController(locationService);
            
            //Act
            var actionResult = controller.GetLocationsAsync(requestDto);

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnLocations = result.Value as FindLocationsReponseDto;
            returnLocations.result.Should().HaveCountLessOrEqualTo((int)requestDto.MaxResults);
        }
    }
}
