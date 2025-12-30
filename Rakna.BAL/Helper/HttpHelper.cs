using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Helper
{
    public class HttpHelper
    {
        public static string? GetToken(HttpContext httpContext)
        {
            string? token = httpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            return token;
        }

        public static string? GetOtpFromQuery(HttpContext httpContext)
        {
            // Try to get the OTP value from query parameters
            if (httpContext.Request.Query.TryGetValue("otp", out var otpValues) && otpValues.Count > 0)
            {
                // Assuming there's only one 'otp' query parameter
                return otpValues[0];
            }
            return null; // Return null if no 'otp' query parameter is found
        }

        public static string? GetTokenHub(HttpContext httpContext)
        {
            string? token = httpContext.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            return token;
        }
    }
}