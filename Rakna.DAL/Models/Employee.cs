using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models
{
    public class Employee : ApplicationUser
    {
        [Required]
        public string NationalID { get; set; }
        [Required]
        public double Salary { get; set; }
        public DateTime DateOfJoining { get; set; }

        [ForeignKey("Garage")]
        public int GarageId { get; set; }
        public virtual Garage Garage { get; set; }
      

    }
}
