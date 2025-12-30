using Rakna.BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageDto
{
    public class ParkingSessionDetailsDto
    {
        public string DriverName { get; set; } = "Nan";
        public string PlateLetters { get; set; }
        public string PlateNumbers { get; set; }
        public double Bill { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public string SessionDuration { get; set; }
        [JsonIgnore]
        public int ParkingSessionId { get; set; }

    }
}
