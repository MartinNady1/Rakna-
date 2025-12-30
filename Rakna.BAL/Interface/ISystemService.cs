using Rakna.BAL.DTO.AIDto;
using Rakna.BAL.DTO.ReportDto;
using Rakna.BAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface ISystemService
    {
        Task<GeneralResponse> AddConfidenceAsync(List<DetectionResultDto> results,string token);

    }
}
