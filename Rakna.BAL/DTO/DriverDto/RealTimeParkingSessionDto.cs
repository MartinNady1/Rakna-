using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.DriverDto
{
    public class RealTimeParkingSessionDto
    {
        public string GarageID { get; set; }
        public string DriverName { get; set; }
        public string GarageStreet { get; set; }
        public string GarageCity { get; set; }
        public string SessionStart { get; set; }
        public TimeSpan SessionTime { get; set; }
        public double SessionPrice { get; set; }
        public string carplate { get; set; }
    }
}
