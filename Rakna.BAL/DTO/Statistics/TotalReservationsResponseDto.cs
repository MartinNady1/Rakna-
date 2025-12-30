using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.Statistics
{
    public class TotalReservationsResponseDto
    {
        public int NumberOfReservations { get; set; }
        public double SumReservationMoney { get; set; }
        public List<int> PeakHoursOfTheDay { get; set; }
        public int ReservationThatIsActuallyUsed { get; set; }
/*        public TimeSpan AvgTimeBeforeReservationUsed { get; set; }
        public TimeSpan AvgTimeAfterReservationStarts { get; set; }*/
    }
}