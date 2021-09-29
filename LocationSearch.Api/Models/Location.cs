using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocationSearch.Api.Models
{
    public class Location
    {   
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }
        
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [NotMapped]
        public double Distance { get; set; }
    }
}
