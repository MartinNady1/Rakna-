using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.Helper;
using Rakna.DAL.DTO.GarageStaffDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface IGarageStaffService
    {
        Task<IEnumerable<GettAllSessionDto>> GetCurrentParkingSessionsAsync(string? token);
        Task<IEnumerable<ReservationDetailsDto>> GetAllReservationsAsync(string? token);
        Task<GarageScreenDto> AvailableSpaces(string? token);
        Task<GeneralResponse> StartParkingSessionAsync(StartParkingSessionDto parkingSessionDto,string? token);
        Task<ParkingSessionDetailsDto> EndParkingSessionAsync(EndParkingSessionDto endParkingSessionDto,string? token);

    }
}
