using LazyCache;
using LocationSearch.Api.DataAccess;
using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private double CalculateDistance(Location firstLocation, Location secondLocation)
        {
            var rlat1 = Math.PI * firstLocation.Latitude / 180;
            var rlat2 = Math.PI * secondLocation.Latitude / 180;
            var rlon1 = Math.PI * firstLocation.Longitude / 180;
            var rlon2 = Math.PI * secondLocation.Longitude / 180;
            var theta = firstLocation.Longitude - secondLocation.Longitude;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }

        public IEnumerable<Location> FindNearestLocations(FindLocationsRequestDto locationDto)
        {
            Location location = new() { Address = "", Latitude = (double)locationDto.Latitude, Longitude = (double)locationDto.Longitude };
            int offset = 0;
            int limit = defaultSettings.ChunkSize;
            int maximumNumberOfResults = locationDto.MaxResults ?? defaultSettings.MaximumNumberOfResults;

            IList<Location> locationChunk;
            List<Location> nearestLocations = new List<Location>();
            
            do
            {
                locationChunk = memoryCache.GetOrAdd($"{offset}", entry => {
                    entry.SetSlidingExpiration(TimeSpan.FromHours(defaultSettings.SlidingExpiration));
                    entry.SetAbsoluteExpiration(TimeSpan.FromHours(defaultSettings.AbsoluteExpiration));

                    return context.Locations.OrderBy(l => l.Address).Skip(offset).Take(limit).ToList(); 
                });

                offset += limit;
                for (int i = 0; i < locationChunk.Count; i++)
                {
                    var distance = CalculateDistance(location, locationChunk[i]);
                    if (distance <= (double)locationDto.MaxDistance)
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
