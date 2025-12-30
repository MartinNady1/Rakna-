using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface IHistoryService // هنا هنخزن بس
    {
        Task CreateReportHistoryAsync(string reportDetails);
        Task UpdateReportStatusHistoryAsync(int reportId);
        Task EndParkingSessionHistoryAsync(int parkingSessionId);
        Task CountStaffHistoryAsync(int staffCount);
        Task EditGarageHourPriceHistoryAsync(int garageId, double newHourPrice);
        Task ReservationStatusHistoryAsync(int reservationId, bool isUsed);
    }

}
