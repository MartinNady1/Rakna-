using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models
{
    public class GarageAdmin : ApplicationUser
    {
        public string AdminNationalID { get; set; }

    
        [ForeignKey("Garage")]
        public int GarageId { get; set; }
        public virtual Garage Garage { get; set; }


    }
}
