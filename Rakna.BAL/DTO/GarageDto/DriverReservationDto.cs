using Rakna.BAL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageDto
{
    public class DriverReservationDto
    {
        public int GarageId { get; set; }
        public DateTime dateTime { get; set; } = DateTime.UtcNow.AddHours(3);
    }
}