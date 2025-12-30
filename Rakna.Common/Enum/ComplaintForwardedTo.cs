using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.Common.Enum
{
    public enum ComplaintForwardedTo
    {
        [Display(Name = "Technical Support")]
        technicalsupport,

        [Display(Name = "Garage Admin")]
        garageadmin
    }
}