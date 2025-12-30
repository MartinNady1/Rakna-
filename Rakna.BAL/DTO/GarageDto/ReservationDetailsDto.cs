using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageDto
{
    public class ReservationDetailsDto
    {
        public int ReservationID { get; set; }
        public string NationalID { get; set; }
        public string GarageLocation { get; set; }
        public DateTime? ReservationDate { get; set; }
        public double ReservationCost { get; set; }
        public string DriverName { get; set; }
    }
}
