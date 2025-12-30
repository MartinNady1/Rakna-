using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.Statistics
{
    public class TotalRevenueResponseDto
    {
        public double SumRequiredPayments { get; set; }
        public double SumActualPayments { get; set; }
        public double ProfitFromOverpay { get; set; }
        public int NumberOfCardPayments { get; set; }
        public int NumberOfCashPayments { get; set; }
        public int NumberOfMobilePayments { get; set; }
    }
}