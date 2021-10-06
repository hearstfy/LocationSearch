using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LazyCache;
using LazyCache.Mocks;
using LocationSearch.Api.DataAccess;
using LocationSearch.Api.Dtos;
using LocationSearch.Api.Models;
using LocationSearch.Api.Services;
using LocationSearch.Api.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace LocationSearch.UnitTests
{
    public class LocationServiceTests
    {
        private readonly Random rand = new();
        [Fact]
        public async Task FindNearestLocations_ReturnsLocationsList()
        {
            var dbContext = await GetDatabaseContext();
            var settings = new DefaultSettings(){
                AbsoluteExpiration = 2,
                ChunkSize = 100000,
                MaximumNumberOfResults = 100,
                SlidingExpiration = 1
            };

            var mockSettings = new Mock<IOptionsSnapshot<DefaultSettings>>(); 
            mockSettings.Setup(o => o.Value).Returns(settings);

            var service = new LocationService(dbContext, mockSettings.Object, new MockCachingService());
            var dto = new FindLocationsRequestDto() { Latitude = 53.63764880, Longitude = 4.73995440, MaxDistance = 100, MaxResults = 100 };
            var result = service.FindNearestLocations(dto);

            Assert.IsAssignableFrom<IEnumerable<Location>>(result).Should().NotBeNull();
        }

        private async Task<LocationDataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<LocationDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new LocationDataContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Locations.CountAsync() <= 0)
            {
                for (int i = 1; i <= 100; i++)
                {
                    databaseContext.Locations.Add(new Location()
                    {
                        Address = "",
                        Latitude = rand.NextDouble() * (90 + 90) - 90,
                        Longitude = rand.NextDouble() * (180 + 180) - 180,
                    });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
    }
}