using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models
{
    public class Vehicle
    {
        [Required]
        public int VehicleId { get; set; }
        [Required]
        [StringLength(4)]
        public string PlateLetters { get; set; }
        [Required]
        [StringLength(4)]
        public string PlateNumbers { get; set; }
        [ForeignKey("Driver")]
        public string DriverId { get; set; }
        public virtual Driver Driver { get; set; }
      

    }
}
