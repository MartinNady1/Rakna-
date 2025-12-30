using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageAdminDtos
{
    public class StaffSalariesDto
    {
        public int DaysUntilPayment { get; set; }
        public List<StaffSalariesListDto> Staffs { get; set; }
    }
}