using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rakna.Common.Enum;

namespace Rakna.DAL.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string? CustomerServiceId { get; set; }
        public bool IsFixed { get; set; }
        public ComplaintType ReportType { get; set; }
        public string? ReportReceiver { get; set; }
        public string ReportMessege { get; set; }
        public string ReporterId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}