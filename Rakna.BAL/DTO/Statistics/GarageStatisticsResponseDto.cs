using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.Statistics
{
    public class GarageStatisticsResponseDto
    {
        public string GarageId { get; set; }
        public AverageParkingDurationResponseDto AverageParkingDuration { get; set; }
        public TotalRevenueResponseDto TotalRevenue { get; set; }
        public TotalReservationsResponseDto TotalReservations { get; set; }
        public TotalSalaryPaidResponseDto TotalSalaryPaid { get; set; }
        public StaffActivityRatingResponseDto StaffActivityRating { get; set; }
        public ComplaintsStatisticsResponseDto ComplaintsStatistics { get; set; }
        public ComplaintsStatisticsResponseDto ComplaintsStatistics_nonsolved { get; set; }
        public PeakParkingHoursDto PeakParkingHours { get; set; }
        public ReservedVsNonReservedParkingUsageDto ReservedVsNonReservedParkingUsage { get; set; }
    }
}