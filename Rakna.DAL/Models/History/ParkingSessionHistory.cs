using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rakna.Common.Enum;

namespace Rakna.DAL.Models.History
{
    public class ParkingSessionHistory
    {
        [Key]
        public int ParkingSessionId { get; set; }

        public int? VehicleId { get; set; } // Adjust based on your Vehicle model
        public string PlateLetters { get; set; }
        public string PlateNumbers { get; set; }
        public int GarageId { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public double HourlyPrice { get; set; }
        public double RequiredPayment { get; set; }
        public double ActualPayment { get; set; }
        public PaymentMethod PaymentType { get; set; } // Cash or Card or Mobile
        public bool reserved { get; set; }
        public string StaffId { get; set; }

        [ForeignKey("History")]
        public int HistoryId { get; set; }

        public virtual History History { get; set; }
    }
}