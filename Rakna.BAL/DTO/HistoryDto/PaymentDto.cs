using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Rakna.Common.Enum;

namespace Rakna.BAL.DTO.HistoryDto
{
    public class PaymentDto
    {
        [JsonIgnore]
        public bool isSuccess { get; set; }

        public string TransactionId { get; set; }
        public double AmountPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime Timestamp { get; set; }
        public string CustomerIdOrContactInfo { get; set; }
    }
}