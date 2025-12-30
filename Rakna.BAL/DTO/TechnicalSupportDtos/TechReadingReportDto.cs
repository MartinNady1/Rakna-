using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.TechnicalSupportDtos
{
    public class TechReadingReportDto
    {
        public int ReportId { get; set; }
        public ComplaintType ReportType { get; set; }
        public string ReportMessage { get; set; }
    }
}