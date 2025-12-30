using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models
{
    public class Garage
    {
        public int GarageId { get; set; }
        public string garageName { get; set; } 
        public double HourPrice { get; set; }
        public string street {  get; set; }
        public string city { get; set; }
        [AllowNull]
        public string? Longitude { get; set; }
        [AllowNull]
        public string? Latitude { get; set; }
        public int AvailableParkingSlots { get; set; }
        public int TotalParkingSlots { get; set; }
        public virtual IEnumerable<Employee>? Employee { get; set; } 

        public virtual IEnumerable<ParkingSession>? ParkingSessions { get; set; }
        public virtual IEnumerable<Reservation>? Reservations { get; set; }
        public virtual GarageAdmin GarageAdmin { get; set; }
     
    }
}
