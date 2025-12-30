using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Rakna.BAL.DTO;
using Rakna.BAL.DTO.DriverDto;
using Rakna.BAL.Helper;
using Rakna.BAL.Interfaces;
using Rakna.DAL;
using Rakna.DAL.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Rakna.Common.Enum;
using Rakna.BAL.Interface;
using Rakna.BAL.DTO.OtpsDto;

namespace Rakna.BAL.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Jwt _jwt;
        private readonly IEmailSendService _oTPService;
        private readonly IDecodeJwt _decodeJwt;
        private SpHelper spvalidate = new SpHelper();

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<Jwt> jwt, IUnitOfWork unitOfWork, IEmailSendService oTPService, IDecodeJwt decodeJwt)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwt = jwt.Value;
            _oTPService = oTPService;
            _decodeJwt = decodeJwt;
        }

        public async Task<AuthApiResponse> LogIn(LoginDto loginDto)
        {
            AuthApiResponse authApiResponse = new AuthApiResponse();
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new AuthApiResponse
                {
                    Message = "Email or Password is incorrect!",
                };
            }
            var isEmailVerified = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailVerified)
            {
                var existotp = await _unitOfWork.EmailOTP.FindAsync(e => e.UserId == user.Id && e.ExpiryTime > DateTime.UtcNow);
                if (existotp is null)
                {
                    var tokenconfirm = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _oTPService.RegisterOTP(user.Id, tokenconfirm);
                }

                return new AuthApiResponse
                {
                    IsAuthenticated = false,
                    Message = "Email is not confirmed yet!",
                };
            }
            var JwtToken = await CreateJwtToken(user);
            var Role = await _userManager.GetRolesAsync(user);

            authApiResponse.IsAuthenticated = true;
            authApiResponse.Message = "User logged in succesfully";
            authApiResponse.Token = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            await _userManager.UpdateAsync(user);
            return authApiResponse;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName",user.FullName),
                new Claim("PhoneNumber",user.PhoneNumber),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
            claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<GeneralResponse> SignUp(RegisterDto registerDto)
        {
            string passwordErrors = spvalidate.isValidPassword(registerDto.Password);
            string usernameErrors = spvalidate.isValidUsername(registerDto.UserName);

            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
                return new GeneralResponse { Message = "Email is already registered!", };

            if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
                return new GeneralResponse { Message = "Username is already used !" };

            if (await _userManager.FindByEmailAsync(registerDto.PhoneNumber) is not null)
                return new GeneralResponse { Message = "Mobile Number is already registered!", };

            if (registerDto.NationalId.Length != 14 || !spvalidate.isStringNumeric(registerDto.NationalId))
                return new GeneralResponse { Message = "National ID is not Valid , try again !" };

            if (!spvalidate.isValidEmail(registerDto.Email))
                return new GeneralResponse { Message = "Email is not valid !" };

            if (passwordErrors.Length > 0)
                return new GeneralResponse { Message = $"{passwordErrors}" };
            if (usernameErrors.Length > 0)
                return new GeneralResponse { Message = $"{usernameErrors}" };

            IdentityResult? SignUpResult = null;
            if (registerDto.Role == UserRole.driver)
            {
                Driver driver = new Driver
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    FullName = registerDto.FullName,
                    NationalNumber = registerDto.NationalId
                };
                SignUpResult = await _userManager.CreateAsync(driver, registerDto.Password);

                if (SignUpResult is null || !SignUpResult.Succeeded)
                {
                    return new GeneralResponse
                    {
                        Message = string.Join(" | ", SignUpResult.Errors.Select(e => e.Description)),
                    };
                }
                await _userManager.AddToRoleAsync(driver, "driver");
                var confirmtoken = await _userManager.GenerateEmailConfirmationTokenAsync(driver);
                var otpresult = await _oTPService.RegisterOTP(driver.Id, confirmtoken);
                if (!otpresult.Success)
                {
                    await _userManager.DeleteAsync(driver);
                    return new GeneralResponse
                    {
                        Message = "Email not registered!",
                    };
                }
            }
            else if (registerDto.Role == UserRole.technicalsupport)
            {
                ApplicationUser technicalsupport = new ApplicationUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    FullName = registerDto.FullName,
                };
                SignUpResult = await _userManager.CreateAsync(technicalsupport, registerDto.Password);
                if (SignUpResult is null || !SignUpResult.Succeeded)
                {
                    return new GeneralResponse
                    {
                        Message = string.Join(" | ", SignUpResult.Errors.Select(e => e.Description)),
                    };
                }
                await _userManager.AddToRoleAsync(technicalsupport, "technicalsupport");
            }
            else
            {
                return new GeneralResponse
                {
                    Message = "Invalid Role",
                };
            }
            return new GeneralResponse
            {
                Success = true,
                Message = "User Created Successfully",
            };
        }

        public async Task<GeneralResponse> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return new GeneralResponse
                    {
                        Success = true,
                        Message = "User Deleted Successfully"
                    };
                }
                else
                {
                    return new GeneralResponse()
                    {
                        Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                    };
                }
            }

            return new GeneralResponse
            {
                Success = false,
                Message = "User not found"
            };
        }

        public async Task<GeneralResponse> IsEmailVerified(string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var user = await _userManager.FindByIdAsync(userId);
            if (userId == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Invalid Token"
                };
            }
            var isEmailVerified = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailVerified)
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Email is not Verified"
                };
            return new GeneralResponse
            {
                Success = true,
                Message = "Email Verified"
            };
        }

        public async Task<GeneralResponse> RequestPasswordReset(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new GeneralResponse { Success = false, Message = "User not found." };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var IsSent = await _oTPService.MakeResetPasswordOTP(user.Id, token);
            if (!IsSent.Success)
                return new GeneralResponse { Success = false, Message = "Failed to send password reset token." };
            // Send email with this token
            // For example, generate a link to your frontend page for password reset:
            // var callbackUrl = $"http://{your_frontend_domain}/reset-password?token={HttpUtility.UrlEncode(token)}&email={HttpUtility.UrlEncode(email)}";
            // Send the callbackUrl via email to the user

            return new GeneralResponse { Success = true, Message = "Password reset token sent." };
        }

        public async Task<GeneralResponse> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return new GeneralResponse { Success = false, Message = "User not found." };
            }

            var result = await _oTPService.ResetPasswordOTP(resetPasswordDto);
            if (!result.Success)
            {
                return new GeneralResponse { Success = false, Message = "Failed to reset password." };
            }

            return new GeneralResponse { Success = true, Message = "Password has been reset." };
        }
    }
}