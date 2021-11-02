using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult GetLocations([FromBody] FindLocationsRequestDto locationDto)
        {
            try
            {
                return Ok(new FindLocationsReponseDto
                {
                    result = locationService.FindNearestLocations(locationDto)
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
