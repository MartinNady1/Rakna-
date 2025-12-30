using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.GarageDto
{
    public class EndParkingSessionDto
    {
        public string PlateLetters { get; set; }
        public string PlateNumbers { get; set; }
        public double Payment { get; set; }

        /// <summary>The type of payment ("Cash" or "Card" or "Mobile"). </summary>
        public PaymentMethod PaymentType { get; set; } //Cash or Card or Mobile
    }
}