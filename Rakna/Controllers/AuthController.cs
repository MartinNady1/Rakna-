using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rakna.BAL.DTO;
using Rakna.BAL.DTO.OtpsDto;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.BAL.Interfaces;
using Serilog;
using System.Text.Json;

namespace Rakna.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailSendService _OTPService;

        public AuthController(IAuthService authService, IEmailSendService OTPService)
        {
            _authService = authService;
            _OTPService = OTPService;
        }

        /// <summary>
        /// This is where the Driver is Registering an account on the Mobile Application
        /// </summary>

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.SignUp(model);
            if (result.Message != "User Created Successfully")
            {
                return BadRequest(JsonSerializer.Serialize(result.Message));
            }
            return Ok(result);
        }

        /// <summary>
        /// This is where Every Actor Will Login using Email & Password
        /// </summary>
        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LogIn(loginDto);
            return Ok(result);
        }

        /// <summary>
        /// محدش يقرب من هنا
        /// </summary>
        /// <remarks>
        ///⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣤⣤⣤⣤⣤⣶⣦⣤⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⡿⠛⠉⠙⠛⠛⠛⠛⠻⢿⣿⣷⣤⡀⠀⠀⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⠀⣼⣿⠋⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⠈⢻⣿⣿⡄⠀⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⣸⣿⡏⠀⠀⠀⣠⣶⣾⣿⣿⣿⠿⠿⠿⢿⣿⣿⣿⣄⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⣿⣿⠁⠀⠀⢰⣿⣿⣯⠁⠀⠀⠀⠀⠀⠀⠀⠈⠙⢿⣷⡄⠀<br/>
        ///⠀⠀⣀⣤⣴⣶⣶⣿⡟⠀⠀⠀⢸⣿⣿⣿⣆⠀⠀⠀تمام⠀⠀⠀⠀⠀⣿⣷⠀<br/>
        ///⠀⢰⣿⡟⠋⠉⣹⣿⡇⠀⠀⠀⠘⣿⣿⣿⣿⣷⣦⣤⣤⣤⣶⣶⣶⣶⣿⣿⣿⠀<br/>
        ///⠀⢸⣿⡇⠀⠀⣿⣿⡇⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠃⠀<br/>
        ///⠀⣸⣿⡇⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠉⠻⠿⣿⣿⣿⣿⡿⠿⠿⠛⢻⣿⡇⠀⠀<br/>
        ///⠀⣿⣿⠁⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣧⠀⠀<br/>
        ///⠀⣿⣿⠀⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⠀⠀<br/>
        ///⠀⣿⣿⠀⠀⠀⣿⣿⡇⠀⠀كله هيتحاسب⠀⠀⠀⠀⠀⠀⢸⣿⣿⠀⠀<br/>
        ///⠀⢿⣿⡆⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⠃⠀⠀<br/>
        ///⠀⠀⠛⢿⣿⣿⣿⣿⣇⠀⠀⠀⠀⠀⣰⣿⣿⣷⣶⣶⣶⣶⠶⠀⢠⣿⣿⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⣿⣿⠀⠀⠀⠀⠀⣿⣿⡇⠀⣽⣿⡏⠁⠀⠀⢸⣿⡇⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⣿⣿⠀⠀⠀⠀⠀⣿⣿⡇⠀⢹⣿⡆⠀⠀⠀⣸⣿⠇⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⢿⣿⣦⣄⣀⣠⣴⣿⣿⠁⠀⠈⠻⣿⣿⣿⣿⡿⠏⠀⠀⠀⠀<br/>
        ///⠀⠀⠀⠀⠀⠀⠀⠈⠛⠻⠿⠿⠿⠿⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀<br/>
        ///
        /// </remarks>
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _authService.DeleteUser(userId);
            return Ok(result.Result);
        }

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string otp)
        {
            var result = await _OTPService.RegisterVerifyOTP(otp);

            if (!result.Success)
                return Redirect("https://raknaapi.azurewebsites.net/HTMLs/RegisterOtpVerificationDeclinedPage.html");
            return Redirect("https://raknaapi.azurewebsites.net/HTMLs/RegisterOtpVerificationAcceptedPage.html");
        }

        /// <summary>
        /// Check if the logged in account is Confirmed or not
        /// </summary>
        [HttpGet("IsEmailVerified")]
        public async Task<IActionResult> IsEmailVerified()
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            if (token == null)
            {
                return NotFound("User not found.");
            }
            var result = await _authService.IsEmailVerified(token);
            return Ok(result);
        }

        /// <summary>
        /// نطلب ايد الباسوورد
        /// </summary>
        [HttpPost("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.RequestPasswordReset(email);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        /// <summary>
        /// اللينك اللي بيجي من الايميل
        /// </summary>
        [HttpGet("SendResetPassword")]
        public async Task<IActionResult> SendResetPassword([FromQuery] string otp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _OTPService.PasswordResetVerifyOTP(otp);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// نغير الباسوورظ
        /// </summary>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ResetPassword(resetPasswordDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}