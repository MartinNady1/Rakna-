using Newtonsoft.Json.Linq;
using Rakna.BAL.DTO.BulkMailDto;
using Rakna.BAL.DTO.DriverDto;
using Rakna.BAL.DTO.GarageAdminDtos;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.Statistics;
using Rakna.BAL.Helper;
using Rakna.DAL.DTO.GarageStaffDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface IGarageAdminService
    {
        Task<GeneralResponse> AddStaff(AddStaffDTO addStaffDto, string? token);

        Task<GeneralResponse> EditStaff(string id, AddStaffDTO addStaffDto);

        Task<GeneralResponse> DeleteStaff(string id);

        Task<GeneralResponse> EditHourPrice(int newHourPrice, string? token);

        ICollection<StaffDto> getAllStaff(string? token);

        /*        Task<GarageReportDto> GarageReport(string token, DateOnly date);
        */

        Task<ICollection<GettAllSessionDto>> gettAllSession(string? token);

        Task<ICollection<GetAllStaffpaidSalaryDto>> GetAllStaffpaidSalary(string? token);

        Task<ICollection<GetAllStaffUnpaidSalaryDto>> GetAllStaffUnpaidSalary(string? token);

        Task<StaffSalariesDto> GetAllStaffSalaries(string token);

        Task<GeneralResponse> PaySalary(string id);

        Task<GarageDataDto> GetGarageData(string? token);

        Task<ICollection<BulkListDto>> getAllStaffbulk(string? token);

        Task<AverageParkingDurationResponseDto> CalculateAverageParkingDurationAsync(StatisticsRequestDto request, string? token);

        Task<TotalRevenueResponseDto> CalculateTotalRevenueAsync(StatisticsRequestDto request, string? token);

        Task<TotalReservationsResponseDto> CalculateTotalReservationsAsync(StatisticsRequestDto request, string? token);

        Task<TotalSalaryPaidResponseDto> CalculateTotalSalaryPaidAsync(StatisticsRequestDto request, string? token);

        Task<StaffActivityRatingResponseDto> CalculateStaffActivityRatingAsync(StatisticsRequestDto request, string token);

        Task<ComplaintsStatisticsResponseDto> CalculateComplaintsStatisticsAsync(StatisticsRequestDto request, string token);

        Task<PeakParkingHoursDto> GetPeakParkingHoursAsync(StatisticsRequestDto request, string token);

        Task<ReservedVsNonReservedParkingUsageDto> GetReservedVsNonReservedParkingUsageAsync(StatisticsRequestDto request, string token);

        Task<GarageStatisticsResponseDto> CalculateGarageStatisticsAsyncSlow(StatisticsRequestDto request, string? token);

        Task<GarageStatisticsResponseDto> CalculateGarageStatisticsAsyncFast(StatisticsRequestDto request, string? token);
    }
}