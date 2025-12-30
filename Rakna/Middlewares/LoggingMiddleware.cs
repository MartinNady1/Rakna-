using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Rakna.BAL.Interface;
using Rakna.DAL.Models;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Rakna.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public IDecodeJwt _decode { get; }

        public LoggingMiddleware(RequestDelegate next, IDecodeJwt decode)
        {
            _next = next;
            _decode = decode;
        }

        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            string authorizationHeader = context.Request.Headers["Authorization"];
            string token = null;
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                token = authorizationHeader.Substring("Bearer ".Length).Trim();
            }
            ApplicationUser? user = null;

            try
            {
                string? id = null;
                if (token != null)
                {
                    id = _decode.GetUserIdFromToken(token);
                }
                user = await userManager.FindByIdAsync(id);

                var requestBodyStream = new MemoryStream();
                var originalRequestBody = context.Request.Body;
                await context.Request.Body.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                var url = UriHelper.GetDisplayUrl(context.Request);
                var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                Log.Information("Request details: {@RequestDetails}", new
                {
                    RequestMethod = context.Request.Method,
                    RequestBody = requestBodyText,
                    RequestUrl = url,
                    UserName = user?.UserName ?? "Anonymous"
                });

                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;

                var bodyStream = context.Response.Body;
                var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;
                await _next(context);
                context.Request.Body = originalRequestBody;
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

                Log.Information("Response details: {@ResponseDetails}", new
                {
                    ResponseBody = responseBody
                });

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(bodyStream);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception occurred: {@ExceptionDetails}", new
                {
                    Endpoint = context.GetEndpoint()?.DisplayName ?? "Unknown endpoint",
                    UserName = user?.UserName ?? "Anonymous",
                    RequestMethod = context.Request.Method,
                    RequestPath = context.Request.Path
                });
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
