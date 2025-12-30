using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rakna.BAL.DTO;
using Rakna.BAL.DTO.AIDto;
using Rakna.BAL.DTO.BulkMailDto;
using Rakna.BAL.DTO.DriverDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.Statistics;
using Rakna.BAL.DTO.TechnicalSupportDtos;
using Rakna.BAL.Helper;
using Rakna.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface ITechnicalSupportService
    {
        Task<ICollection<GetGarageDto>> GetAllGarages();

        Task<GeneralResponse> AddGarage(GarageDto garageDto);

        Task<GeneralResponse> UpdateGarage(int id, GarageDto garageDto);

        Task<GeneralResponse> DeleteGarage(int id);

        Task<GeneralResponse> RegisterUser(AddUserDto Model);

        Task<GeneralResponse> EditStaffBasedOnRole(string id, EditUserDto model);

        Task<GeneralResponse> DeleteUser(string id);

        Task<IEnumerable<TechReadingReportDto>> GetAllReports();
        Task<IEnumerable<ListUserDto>> GetAllUsers();
        Task<IEnumerable<DriverDto>> GetAllDriverID();
        Task<IEnumerable<GarageStatisticsResponseDto>> GetAllGarageStatistics(StatisticsRequestDto request);
        Task<ICollection<BulkListDto>> GetAllUsersWithRolesAsync(string? token);
        Task<GarageStatisticsResponseDto> CalculateGarageStatisticsAsyncFast(StatisticsRequestDto request, Garage garage);
        Task<ResponseAiConfidence> GetConfidence();
    }
}