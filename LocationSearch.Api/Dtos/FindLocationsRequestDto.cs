using System;
using System.ComponentModel.DataAnnotations;

namespace LocationSearch.Api.Dtos
{
    public class FindLocationsRequestDto
    {
        [Required]
        [Range(-90.0, 90.0)]
        public double? Latitude { get; set; }

        [Required]
        [Range(-180.0, 180.0)]
        public double? Longitude { get; set; }

        [Required]
        [Range(0, 12756000)]
        public double? MaxDistance { get; set; }

        public int? MaxResults { get; set; }
    }
}
