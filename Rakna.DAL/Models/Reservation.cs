using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models
{
    public class Reservation
    {
         public int ReservationId { get; set; }
        [ForeignKey("Driver")]
         public string DriverID {  get; set; }
         public virtual Driver Driver { get; set; }
        [ForeignKey("Garage")]
        public int GarageId { get; set; }
        public virtual Garage Garage { get; set; }
        public required DateTime DateTime { get; set; }

    }
}
