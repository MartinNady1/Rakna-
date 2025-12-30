using Rakna.BAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Rakna.BAL.DTO.OtpsDto;

namespace Rakna.BAL.Service
{
    public class MailService : ImailSenderService
    {
        private readonly EmailSettings _emailSettings;

        public MailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(List<string> toEmails, string subject, string content)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailSettings.SmtpHost)
                {
                    Port = _emailSettings.SmtpPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSSL,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail),
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(new MailAddress(toEmails[0]));
                for (int i = 0; i < toEmails.Count - 1; i++)
                    mailMessage.Bcc.Add(toEmails[i]);

                await smtpClient.SendMailAsync(mailMessage);
                await Task.Delay(3000);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}