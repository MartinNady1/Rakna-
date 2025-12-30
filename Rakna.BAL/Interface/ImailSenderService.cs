using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface ImailSenderService
    {
        Task<bool> SendEmailAsync(List<string> toEmail, string subject, string content);
    }
}