using LazyCache;
using LocationSearch.Api.DataAccess;
using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSearch.Api.Services
{

    public class LocationService : ILocationService
    {
        private readonly LocationDataContext context;
        private readonly DefaultSettings defaultSettings;
        private readonly IAppCache memoryCache;
        public LocationService(LocationDataContext context, IOptionsSnapshot<DefaultSettings> defaultSettings, IAppCache memoryCache)
        {
            this.context = context;
            this.defaultSettings = defaultSettings.Value;
            this.memoryCache = memoryCache;

        }
        private double CalculateDistance(double latitude, double longitude,Location location)
        {
            var rlat1 = Math.PI * latitude / 180;
            var rlat2 = Math.PI * location.Latitude / 180;
            var rlon1 = Math.PI * longitude / 180;
            var rlon2 = Math.PI * location.Longitude / 180;
            var theta = longitude - location.Longitude;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }

        public async Task<IEnumerable<Location>> FindNearestLocations(double latitude, double longitude, double maxDistance, int? maxResults)
        {
            int offset = 0;
            int limit = defaultSettings.ChunkSize;
            int maximumNumberOfResults = maxResults ?? defaultSettings.MaximumNumberOfResults;

            IList<Location> locationChunk;
            List<Location> nearestLocations = new List<Location>();
            
            
            do
            {
                locationChunk = await memoryCache.GetOrAddAsync<List<Location>>($"{offset}", entry => {
                    entry.SetSlidingExpiration(TimeSpan.FromHours(defaultSettings.SlidingExpiration));
                    entry.SetAbsoluteExpiration(TimeSpan.FromHours(defaultSettings.AbsoluteExpiration));
        
                    return context.Locations.OrderBy(l => l.Address).Skip(offset).Take(limit).ToListAsync(); 
                });

                offset += limit;
                for (int i = 0; i < locationChunk.Count; i++)
                {
                    var distance = CalculateDistance(latitude, longitude, locationChunk[i]);
                    if (distance <= maxDistance)
                    {
                        locationChunk[i].Distance = Math.Round(distance, 2);
                        nearestLocations.Add(locationChunk[i]);
                    }
                }
            } while (locationChunk.Count > 0);
            
            nearestLocations.Sort((x, y) => x.Distance.CompareTo(y.Distance));
            return nearestLocations.Take(maximumNumberOfResults);
        }
    }
}
