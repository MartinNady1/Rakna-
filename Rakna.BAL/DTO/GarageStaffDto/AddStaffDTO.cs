using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageStaffDto
{
    public class AddStaffDTO
    {
        [Required, StringLength(100)]
        public string name { get; set; }

        [StringLength(14)]
        public string NationalId { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public double salary { get; set; }
    }
}