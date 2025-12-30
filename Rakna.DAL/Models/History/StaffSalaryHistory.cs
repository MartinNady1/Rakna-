using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models.History
{
    public class StaffSalaryHistory
    {
        [Key]
        public int StaffSalaryId { get; set; }
        public string StaffName { get; set; }
        public string StaffEmail { get; set; }
        public DateTime CollectTime { get; set; }
        public double Amount { get; set; }

        [ForeignKey("History")]
        public int HistoryId { get; set; }
        public virtual History History { get; set; }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; } // Ensure this matches your Employee ID type
        public virtual Employee Employee { get; set; } // Navigation property
    }
}
