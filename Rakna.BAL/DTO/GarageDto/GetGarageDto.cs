using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageDto
{
    public class GetGarageDto
    {
        public int GarageId {  get; set; }
        public string GarageName { get; set; }
        public double HourPrice { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public int AvailableSpaces { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public int TotalSpaces { get; set; }
        public bool HasAdmin { get; set; }
    }
}
