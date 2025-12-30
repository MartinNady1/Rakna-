using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageAdminDtos
{
    public class GetAllStaffpaidSalaryDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CollectedDate { get; set; }
        public double Amount { get; set; }
    }
}
