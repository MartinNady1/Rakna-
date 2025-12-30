using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.OtpsDto;
using Rakna.BAL.DTO.Statistics;
using Rakna.BAL.DTO.TechnicalSupportDtos;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.Common;
using Rakna.Common.Enum;
using Rakna.DAL;
using Rakna.DAL.Interfaces;
using Rakna.DAL.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Service
{
    public class EmailSendService : IEmailSendService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ImailSenderService _emailSender;
        private readonly IDecodeJwt _decodeJwt;
        private readonly EmailSettings _emailSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public EmailSendService(UserManager<ApplicationUser> userManager,
                          ImailSenderService emailSender,
                          IDecodeJwt decodeJwt,
                          IOptions<EmailSettings> emailSettings, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _decodeJwt = decodeJwt;
            _emailSettings = emailSettings.Value;
            _unitOfWork = unitOfWork;
            _env = env;
        }

        /*        public async Task<GeneralResponse> UpdateMailOTP(string? token, string newemail)
                {
                    var userId = _decodeJwt.GetUserIdFromToken(token);
                    var user = await _userManager.FindByIdAsync(userId);
                    var driver = await _unitOfWork.Driver.FindAsync(e => e.Id == user.Id);
                    if (user == null)
                    {
                        return new GeneralResponse { Success = false, Message = "User not found." };
                    }

                    // Generate OTP
                    var otpEntry = new EmailOTP
                    {
                        DriverId = userId,
                        Driver = driver
                    };
                    await _unitOfWork.EmailOTP.AddAsync(otpEntry);
                    await _unitOfWork.SaveChangeAsync();

                    var emailContent = $"Your OTP for email change is: {otpEntry.OTP}. This OTP will expire in 5 minutes.";
                    await _emailSender.SendEmailAsync(newemail, "OTP for Email Change", emailContent);

                    return new GeneralResponse { Success = true, Message = "OTP has been sent." };
                }
        */

        public async Task<GeneralResponse> RegisterOTP(string userId, string confirmtoken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            bool type = await _userManager.IsInRoleAsync(user, "driver");

            // Generate OTP
            var otpEntry = new EmailOTP
            {
                UserId = user.Id,
                User = user,
                Token = confirmtoken,
                OTP = type ? new Random().Next(100000, 999999).ToString() : Guid.NewGuid().ToString(),
                OTPType = await _userManager.IsInRoleAsync(user, "driver") ? OtpType.Digits : OtpType.Link
            };
            await _unitOfWork.EmailOTP.AddAsync(otpEntry);
            await _unitOfWork.SaveChangeAsync();

            string link = $"https://raknaapi.azurewebsites.net/api/Auth/VerifyEmail?otp={otpEntry.OTP}";
            string htmlFileName = otpEntry.OTPType == OtpType.Digits ? "RegisterOtpVerificationOtpEmailMessage.html" : "RegisterOtpVerificationLinkEmailMessage.html";

            var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
            }
            var htmlContent = await File.ReadAllTextAsync(filePath);

            // For digit OTPs, replace {otp}. For link OTPs, replace {link}.
            htmlContent = (otpEntry.OTPType == OtpType.Digits) ? htmlContent.Replace("{otp}", otpEntry.OTP.ToString()) : htmlContent = htmlContent.Replace("{link}", link);
            List<string> mail = new List<string>();
            mail.Add(user.Email);
            bool sent = await _emailSender.SendEmailAsync(mail, "Rakna OTP for Email Register", htmlContent);

            if (!sent)
            {
                return new GeneralResponse { Success = false, Message = "OTP Failed!" };
            }
            return new GeneralResponse { Success = true, Message = "OTP has been sent." };
        }

        public async Task<GeneralResponse> RegisterVerifyOTP(string OTP)
        {
            var otp = await _unitOfWork.EmailOTP.FindAsync(e => e.OTP == OTP);
            if (otp == null || otp.ExpiryTime < DateTime.UtcNow)
            {
                return new GeneralResponse { Success = false, Message = "Invalid or expired OTP." };
            }
            var user = await _userManager.FindByIdAsync(otp.UserId);
            var result = await _userManager.ConfirmEmailAsync(user, otp.Token);
            if (!result.Succeeded)
            {
                return new GeneralResponse { Success = false, Message = "Failed to CONFIRM email." };
            }

            _unitOfWork.EmailOTP.Delete(otp);
            await _unitOfWork.SaveChangeAsync();
            var filePath = Path.Combine(_env.WebRootPath, "HTMLs", "RegisterOtpVerificationAcceptedEmailMessage.html");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("RegisterOtpVerificationAcceptedEmailMessage.html file not found.", filePath);
            }
            var htmlContent = await File.ReadAllTextAsync(filePath);
            List<string> mail = new List<string>();
            mail.Add(otp.User.Email);
            bool sent = await _emailSender.SendEmailAsync(mail, "Rakna Email Verified", htmlContent);
            return new GeneralResponse { Success = true, Message = "Email has been updated." };
        }

        public async Task<GeneralResponse> MakeResetPasswordOTP(string userId, string confirmtoken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // Generate OTP
            var otpEntry = new EmailOTP
            {
                UserId = user.Id,
                User = user,
                Token = confirmtoken,
                OTP = new Random().Next(100000, 999999).ToString(),
                OTPType = OtpType.Digits
            };
            await _unitOfWork.EmailOTP.AddAsync(otpEntry);
            await _unitOfWork.SaveChangeAsync();

            string otp = $"{otpEntry.OTP}";
            string htmlFileName = "RegisterOtpVerificationOtpEmailMessage.html";

            var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
            }
            var htmlContent = await File.ReadAllTextAsync(filePath);

            // For digit OTPs, replace {otp}. For link OTPs, replace {link}.
            htmlContent = htmlContent.Replace("{otp}", otp);

            List<string> mail = new List<string>();
            mail.Add(user.Email);
            bool sent = await _emailSender.SendEmailAsync(mail, "Rakna OTP for Reset Password", htmlContent);
            if (!sent)
            {
                return new GeneralResponse { Success = false, Message = "OTP Failed!" };
            }
            return new GeneralResponse { Success = true, Message = "OTP has been sent." };
        }

        public async Task<PasswordResetResponse> PasswordResetVerifyOTP(string OTP)
        {
            var otp = await _unitOfWork.EmailOTP.FindAsync(e => e.OTP == OTP);
            if (otp == null || otp.ExpiryTime < DateTime.UtcNow)
            {
                return new PasswordResetResponse { Success = false };
            }
            var user = await _userManager.FindByIdAsync(otp.UserId);
            if (user == null)
                return new PasswordResetResponse { Success = false };
            return new PasswordResetResponse { Success = true, Token = otp.Token, Email = user.Email, OTP = otp.OTP };
        }

        public async Task<GeneralResponse> ResetPasswordOTP(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new GeneralResponse { Success = false, Message = "User not found." };
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return new GeneralResponse { Success = false, Message = "Failed to reset password." };
            }

            return new GeneralResponse { Success = true, Message = "Password has been reset." };
        }

        public async Task<GeneralResponse> SendBulkEmails(List<string> emailDtos, string Message, string Title, string? token)
        {
            var user = await _userManager.FindByIdAsync(_decodeJwt.GetUserIdFromToken(token));
            var role = await _userManager.GetRolesAsync(user);
            string username = "";
            for (int i = 0; i < user.FullName.Length - 1; i++)
            {
                if (user.FullName[i] == ' ') break;
                username += user.FullName[i];
            }

            string htmlFileName = "BulkMessageSenderEmailMessage.html";

            var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
            }
            var htmlContent = await File.ReadAllTextAsync(filePath);

            htmlContent = htmlContent.Replace("{Title}", Title);
            htmlContent = htmlContent.Replace("{Sender}", (role.FirstOrDefault() == "garageadmin") ? "Admin " + username : "Rakna System ");
            htmlContent = htmlContent.Replace("{Message}", Message);

            bool sent = await _emailSender.SendEmailAsync(emailDtos, "Announcement!👉🏿👈🏿", htmlContent);
            if (!sent)
            {
                return new GeneralResponse { Success = false, Message = "Announcement Failed!" };
            }
            return new GeneralResponse { Success = true, Message = "Announcement Sent!" };
        }

        public async Task<GeneralResponse> SendStaffCreationData(AddStaffDTO Model, string password)
        {
            string htmlFileName = "AccountCreatedEmailMessage.html";

            var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
            }
            var htmlContent = await File.ReadAllTextAsync(filePath);
            htmlContent = htmlContent.Replace("{username}", Model.UserName);
            htmlContent = htmlContent.Replace("{email}", Model.Email);
            htmlContent = htmlContent.Replace("{phoneNumber}", Model.PhoneNumber);
            htmlContent = htmlContent.Replace("{fullName}", Model.name);
            htmlContent = htmlContent.Replace("{nationalID}", Model.NationalId);
            htmlContent = htmlContent.Replace("{salary}", Model.salary.ToString() + "$");
            htmlContent = htmlContent.Replace("{password}", password);

            List<string> mail = new List<string>();
            mail.Add(Model.Email);

            bool sent = await _emailSender.SendEmailAsync(mail, "Account Created!", htmlContent);

            if (!sent)
            {
                return new GeneralResponse { Success = false, Message = "Creation Message Send Failed!" };
            }
            return new GeneralResponse { Success = true, Message = "Creation Message Send Successfully!" };
        }

        public async Task<GeneralResponse> SendGarageAdminCreationData(AddUserDto Model, string password)
        {
            string htmlFileName = "AccountCreatedEmailMessage.html";

            var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
            }
            var htmlContent = await File.ReadAllTextAsync(filePath);
            htmlContent = htmlContent.Replace("{username}", Model.UserName);
            htmlContent = htmlContent.Replace("{email}", Model.Email);
            htmlContent = htmlContent.Replace("{phoneNumber}", Model.PhoneNumber);
            htmlContent = htmlContent.Replace("{fullName}", Model.FullName);
            htmlContent = htmlContent.Replace("{nationalID}", Model.NationalId);
            htmlContent = htmlContent.Replace("{salary}", "$$$$");
            htmlContent = htmlContent.Replace("{password}", password);

            List<string> mail = new List<string>();
            mail.Add(Model.Email);

            bool sent = await _emailSender.SendEmailAsync(mail, "Account Created!", htmlContent);

            if (!sent)
            {
                return new GeneralResponse { Success = false, Message = "Creation Message Send Failed!" };
            }
            return new GeneralResponse { Success = true, Message = "Creation Message Send Successfully!" };
        }

        public async Task<GeneralResponse> SendCustomerServiceCreationData(AddUserDto Model, string password)
        {
            string htmlFileName = "AccountCreatedEmailMessage.html";

            var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
            }
            var htmlContent = await File.ReadAllTextAsync(filePath);
            htmlContent = htmlContent.Replace("{username}", Model.UserName);
            htmlContent = htmlContent.Replace("{email}", Model.Email);
            htmlContent = htmlContent.Replace("{phoneNumber}", Model.PhoneNumber);
            htmlContent = htmlContent.Replace("{fullName}", Model.FullName);
            htmlContent = htmlContent.Replace("{nationalID}", Model.NationalId);
            htmlContent = htmlContent.Replace("{salary}", Model.Salary.ToString() + "$");
            htmlContent = htmlContent.Replace("{password}", password);

            List<string> mail = new List<string>();
            mail.Add(Model.Email);

            bool sent = await _emailSender.SendEmailAsync(mail, "Account Created!", htmlContent);

            if (!sent)
            {
                return new GeneralResponse { Success = false, Message = "Creation Message Send Failed!" };
            }
            return new GeneralResponse { Success = true, Message = "Creation Message Send Successfully!" };
        }

        /*  public async Task<bool> SendGarageStatisticsEmailAsync(GarageStatisticsResponseDto statistics, string recipientEmail)
          {
              string htmlFileName = "GarageStatisticsEmail.html";
              var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
              if (!File.Exists(filePath))
              {
                  throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
              }

              var htmlContent = await File.ReadAllTextAsync(filePath);

              htmlContent = htmlContent.Replace("{AverageDuration}", statistics.AverageParkingDuration.AverageDuration.ToString());
              htmlContent = htmlContent.Replace("{TotalRevenue}", statistics.TotalRevenue.ToString());
              htmlContent = htmlContent.Replace("{TotalReservations}", statistics.TotalReservations.ToString());
              htmlContent = htmlContent.Replace("{TotalSalaryPaid}", statistics.TotalSalaryPaid.ToString());
              htmlContent = htmlContent.Replace("{ComplaintsStatistics}", statistics.ComplaintsStatistics.ToString());
              // Continue for each statistic in your DTO...

              // Send the email using your email service (replace _emailSender with your actual email sender)
              bool sent = await _emailSender.SendEmailAsync(recipientEmail, "Garage Statistics Report", htmlContent);
              return sent;
          }*/
    }
}