using Microsoft.AspNetCore.Mvc;
using Rakna.BAL.DTO.DriverDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.HistoryDto;
using Rakna.BAL.DTO.ReportDto;
using Rakna.BAL.DTO.UpdateDriverDto;
using Rakna.BAL.Helper;
using Rakna.DAL.Models;
using Rakna.DAL.Models.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface IDriverService
    {
        Task<GeneralResponse> AddVehicle(VehicleDto addVehicleDto, string? token);
        Task<IEnumerable<GetVehcileDto>> GetAllVehicle(string? token);
        Task<GeneralResponse> EditVehicle(int VehicleId, VehicleDto EditVehicleDto);
        Task<GeneralResponse> DeleteVehicle(int VehicleId);

        Task<GeneralResponse> Reservation(DriverReservationDto reservationDto, string? token);

        Task<IEnumerable<ReservationDetailsDto>> ReservationDetails(string? token);

        Task<DriverProfileDto> DriverProfileDetails(string? token);
        Task<IEnumerable<RealTimeParkingSessionDto>> RealTimeParkingSessionsDetails(string? token);

        Task<GeneralResponse> UpdateDriverDetails(string? token, UpdateDriverDetailsDto updateDto);

        Task<GeneralResponse> UpdateDriverPassword(string? token, UpdateDriverPasswordDto updateDto);

        IEnumerable<ComplaintsHistoryDto> solvedDriversReport(string? token);

        IEnumerable<GetDriverReportDto> UnsolvedDriversReport(string? token);
        Task<GeneralResponse> CancelReservation(int ResevationId);
        Task<GeneralResponse> EditReservation(string? token, int ResevationId ,DriverReservationDto reservationDto);
    }
}