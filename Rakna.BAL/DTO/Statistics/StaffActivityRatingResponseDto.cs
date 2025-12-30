using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.Statistics
{
    public class StaffActivityRatingDto
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public int NumberOfSessions { get; set; }
    }

    public class StaffActivityRatingResponseDto
    {
        public List<StaffActivityRatingDto> StaffActivities { get; set; } = new List<StaffActivityRatingDto>();
    }
}