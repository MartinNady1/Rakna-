using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.Common.Enum
{
    public enum UserRole
    {
        [Display(Name = "Driver")]
        driver,

        [Display(Name = "Garage Admin")]
        garageadmin,

        [Display(Name = "Garage Staff")]
        garagestaff,

        [Display(Name = "Customer Service")]
        customerservice,

        [Display(Name = "Technical Support")]
        technicalsupport
    }
}