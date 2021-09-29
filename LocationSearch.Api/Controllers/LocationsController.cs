using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LocationSearch.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : Controller
    {
        private readonly ILocationService locationService;
        public LocationsController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpPost]
        public IEnumerable<Location> GetLocations([FromBody] FindLocationsRequestDto locationDto)
        {
            return locationService.FindNearestLocations(locationDto);
        }
    }
}
