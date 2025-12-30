using Rakna.BAL.DTO.GarageDto;
using Rakna.DAL.DTO.GarageStaffDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageAdminDtos
{
    public class GarageReportDto
    {
        public double TotalMoney { get; set; }
        public List<ParkingSessionDetailsDto> parkingSessionDetailsDtos { get; set; }
        
    }
}
