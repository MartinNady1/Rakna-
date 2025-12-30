using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models
{
    public class CustomerService  : ApplicationUser
    {
        public string NationalId { get; set; }
        public double Salary { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }


    }
}
