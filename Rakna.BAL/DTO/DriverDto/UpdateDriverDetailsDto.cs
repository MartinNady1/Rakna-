using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.UpdateDriverDto
{
    public class UpdateDriverDetailsDto
    {
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits")]
        public string NationalId { get; set; }
        [Required, StringLength(100)]
        public string UserName { get; set; }
/*        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }*/
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
