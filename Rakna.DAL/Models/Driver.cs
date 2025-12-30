using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rakna.DAL.Models
{
    public class Driver : ApplicationUser
    {
        [AllowNull]
        public string? NationalNumber { get; set; }
        public virtual ICollection<Vehicle>? Vehicles { get; set; } 
    }

}
