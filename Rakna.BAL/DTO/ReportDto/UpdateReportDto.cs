using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.ReportDto
{
    public class UpdateReportDto
    {
        public string CustomerServiceId { get; set; }
        public bool IsFixed { get; set; }
        public ComplaintType ReportType { get; set; }
        public string? ReportReceiver { get; set; }
        public ComplaintForwardedTo ForwardedTo { get; set; }
    }
}