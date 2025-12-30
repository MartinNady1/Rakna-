using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.TechnicalSupportDtos
{
    public class EditUserDto
    {
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [StringLength(14)]
        public string NationalId { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public int? GarageId { get; set; }
        public double? Salary { get; set; }
    }
}