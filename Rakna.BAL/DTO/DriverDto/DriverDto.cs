using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.DriverDto
{
    public class DriverDto
    {
        public string Name { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits")]
          public string NationalId { get; set; }
          public string Id { get; set; }


    }
 
}
