using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetLocationsAsync([FromBody] FindLocationsRequestDto locationDto)
        {
            try
            {
                return Ok(new FindLocationsReponseDto
                {
                    result = await locationService.FindNearestLocations((double)locationDto.Latitude, (double)locationDto.Longitude, (double)locationDto.MaxDistance, locationDto.MaxResults ?? null)
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new FindLocationsReponseDto
                {
                    result = new List<Location>(),
                    error = e.Message
                });
            }
        }
    }
}
