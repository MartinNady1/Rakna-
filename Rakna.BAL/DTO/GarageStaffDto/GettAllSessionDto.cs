using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.DTO.GarageStaffDto
{
    public class GettAllSessionDto
    {
        public string DriverName { get; set; } = "Nan";
        public string PlateLetters { get; set; }
        public string PlateNumbers { get; set; }
        public double CurrentBill { get; set; }
        public DateTime StartDate { get; set; }
        public double CurSessionDuration_Hours { get; set; }
    }
}
