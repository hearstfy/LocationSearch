using LocationSearch.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSearch.Api.DataAccess
{
    public class LocationDataContext: DbContext
    {
        public LocationDataContext(DbContextOptions<LocationDataContext> options): base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
    }
}
