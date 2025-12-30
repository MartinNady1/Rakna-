using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.OtpsDto;
using Rakna.BAL.DTO.TechnicalSupportDtos;
using Rakna.BAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface IEmailSendService
    {
        /*        Task<GeneralResponse> UpdateMailOTP(string? token, string email);
        */

        Task<GeneralResponse> RegisterOTP(string userId, string confirmtoken);

        Task<GeneralResponse> RegisterVerifyOTP(string OTP);

        Task<GeneralResponse> MakeResetPasswordOTP(string userId, string confirmtoken);

        Task<GeneralResponse> ResetPasswordOTP(ResetPasswordDto model);

        Task<PasswordResetResponse> PasswordResetVerifyOTP(string OTP);

        Task<GeneralResponse> SendBulkEmails(List<string> emailDtos, string Message, string Title, string? token);

        Task<GeneralResponse> SendStaffCreationData(AddStaffDTO Model, string password);

        Task<GeneralResponse> SendGarageAdminCreationData(AddUserDto Model, string password);

        Task<GeneralResponse> SendCustomerServiceCreationData(AddUserDto Model, string password);
    }
}