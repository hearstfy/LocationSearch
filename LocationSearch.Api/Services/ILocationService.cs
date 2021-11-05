using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSearch.Api.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> FindNearestLocations(double latitude, double longitude, double maxDistance, int? maxResults);
    }
}