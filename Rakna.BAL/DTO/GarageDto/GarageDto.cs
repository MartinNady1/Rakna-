using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageDto
{
      public class GarageDto
    {
       
        [Required]
        public string GarageName { get; set; }
        [Required]
        public double HourPrice { get; set; }
        [Required]
        public string street { get; set; }
        [Required]
        public string city { get; set; }

        [Required]
        public string? Longitude { get; set; }
        [Required]
        public string? Latitude { get; set; }
        [Required]
        public int TotalSpaces { get; set; }
    }
}
