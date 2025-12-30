using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models.History
{
    public class ReservationHistory
    {
        [Key]
        public int ReservationId { get; set; }
        public string DriverId { get; set; } 
        public int GarageId { get; set; }
        public DateTime ReservationTime { get; set; }
        public bool UsedOrNot { get; set; }

        [ForeignKey("History")]
        public int HistoryId { get; set; }
        public virtual History History { get; set; }
    }

}
