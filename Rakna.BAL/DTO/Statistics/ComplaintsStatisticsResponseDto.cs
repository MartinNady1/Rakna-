using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.Statistics
{
    public class ComplaintsStatisticsResponseDto
    {
        public int NumberOfComplaintsForwardedToGarage { get; set; }
        public Dictionary<ComplaintType, int> ComplaintsByType { get; set; } = new Dictionary<ComplaintType, int>();
        public TimeSpan AverageResolutionTime { get; set; }
    }
}