using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using System.Collections.Generic;

namespace LocationSearch.Api.Services
{
    public interface ILocationService
    {
        IEnumerable<Location> FindNearestLocations(FindLocationsRequestDto locationDto);
    }
}