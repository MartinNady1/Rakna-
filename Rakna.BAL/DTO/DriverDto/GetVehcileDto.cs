using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.DriverDto
{
    public class GetVehcileDto
    {
        public int Id { get; set; }
        public string PlateLetter {  get; set; }
        public string PlateNumber {  get; set; }
    }
}
