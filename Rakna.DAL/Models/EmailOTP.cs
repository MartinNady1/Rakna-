using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rakna.Common.Enum;

namespace Rakna.DAL.Models
{
    public class EmailOTP
    {
        [Key]
        public string OTPId { get; set; } = Guid.NewGuid().ToString();

        public string OTP { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; } = DateTime.UtcNow.AddMinutes(5);
        public OtpType OTPType { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}