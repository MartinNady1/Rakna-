using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageAdminDtos
{
    public class StaffSalariesListDto
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? LastPayment { get; set; }
        public bool isPaid { get; set; }
        public double Amount { get; set; }
    }
}