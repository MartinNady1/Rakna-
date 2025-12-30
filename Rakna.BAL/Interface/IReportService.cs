using Rakna.BAL.DTO.ReportDto;
using Rakna.BAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface IReportService
    {
        Task<GeneralResponse> CreateReportAsync(AddReportDto reportCreationDto, string? token);

        IEnumerable<ReadReportDto> GetAllReports(int turn);

        IEnumerable<GarageAdminsListDto> GetAllGarageadmin();

        IEnumerable<ReadReportDto> GetReportsBasedOnRoleAsync(string? token);

        Task<GeneralResponse> UpdateReportStatusAsync(int reportId, string? token);

        Task<GeneralResponse> ForwardReport(int reportId, string? token, string reportReceiverId);
    }
}