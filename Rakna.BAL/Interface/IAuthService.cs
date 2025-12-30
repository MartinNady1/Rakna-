using Microsoft.AspNetCore.Mvc;
using Rakna.BAL.DTO;
using Rakna.BAL.DTO.OtpsDto;
using Rakna.BAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralResponse> SignUp(RegisterDto registerDto);

        Task<AuthApiResponse> LogIn(LoginDto loginDto);

        Task<GeneralResponse> DeleteUser(string userId);

        Task<GeneralResponse> IsEmailVerified(string? token);

        Task<GeneralResponse> RequestPasswordReset(string email);

        Task<GeneralResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}