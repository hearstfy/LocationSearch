using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LocationSearch.Api.Models;

namespace LocationSearch.Api.Dtos
{
    public class FindLocationsReponseDto
    {
        public string? error {get;set;}
        public IEnumerable<Location> result {get;set;}
    }
}
