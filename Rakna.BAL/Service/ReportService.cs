using Microsoft.AspNetCore.Identity;
using Rakna.BAL.DTO.ReportDto;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rakna.DAL.Models.History;
using Rakna.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Rakna.BAL.DTO.OtpsDto;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rakna.Common.Enum;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth.OAuth2;


namespace Rakna.BAL.Service
{
    public class ReportService : IReportService
    {
        private readonly ImailSenderService _emailSender;
        private readonly IDecodeJwt _decodeJwt;
        private readonly EmailSettings _emailSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private SpHelper spvalidate = new SpHelper();

        public ReportService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IDecodeJwt decodeJwt, IWebHostEnvironment env, ImailSenderService emailSender, IOptions<EmailSettings> emailSettings)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _decodeJwt = decodeJwt;
            _emailSettings = emailSettings.Value;
            _env = env;
        }
        public async Task<string> LanguageDetect(string x)
        {
            string detectedLanguage = "en";
            using (var httpClient = new HttpClient())
            {
                // Language Detection
                var languageDetectionContent = new StringContent(JsonConvert.SerializeObject(new { q = x }), System.Text.Encoding.UTF8, "application/json");
                var languageDetectionRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://deep-translate1.p.rapidapi.com/language/translate/v2/detect"),
                    Headers =
                {
                    { "x-rapidapi-key", "1f2c5867a9mshfc88a40745f9f12p15e2cfjsn7ef1a39611d3" },
                    { "x-rapidapi-host", "deep-translate1.p.rapidapi.com" },
                },
                    Content = languageDetectionContent
                };

                var languageDetectionResponse = await httpClient.SendAsync(languageDetectionRequest);
                if (languageDetectionResponse.IsSuccessStatusCode)
                {
                    var languageDetectionResult = await languageDetectionResponse.Content.ReadAsStringAsync();
                    var languageDetectionJson = JObject.Parse(languageDetectionResult);
                    detectedLanguage = (string)languageDetectionJson["data"]["detections"][0]["language"];
                }
            }
            return detectedLanguage;
        }
        public async Task<string> LanguageTranslate(string x, string y)
        {
            string translatedMessage = "";
            using (var httpClient = new HttpClient())
            {
                var translationContent = new StringContent(JsonConvert.SerializeObject(new { q = x, source = y, target = "en" }), System.Text.Encoding.UTF8, "application/json");
                var translationRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://deep-translate1.p.rapidapi.com/language/translate/v2"),
                    Headers =
                {
                    { "x-rapidapi-key", "1f2c5867a9mshfc88a40745f9f12p15e2cfjsn7ef1a39611d3" },
                    { "x-rapidapi-host", "deep-translate1.p.rapidapi.com" },
                },
                    Content = translationContent
                };

                var translationResponse = await httpClient.SendAsync(translationRequest);
                if (translationResponse.IsSuccessStatusCode)
                {
                    var translationResult = await translationResponse.Content.ReadAsStringAsync();
                    var translationJson = JObject.Parse(translationResult);
                    translatedMessage = (string)translationJson["data"]["translations"]["translatedText"];
                }
            }
            return translatedMessage;
        }
        public async Task<GeneralResponse> CreateReportAsync(AddReportDto reportCreationDto, string token)
        {
            ComplaintType type = ComplaintType.Other;
            string apiUrl = "https://octopus-intent-rapidly.ngrok-free.app/predict";
            var requestBody = new { text = reportCreationDto.ReportMessage };
            string prediction = "";
            string detectedLanguage = "en"; // Default language
            string translatedMessage = "";

            try
            {
                detectedLanguage = await LanguageDetect(reportCreationDto.ReportMessage);
                translatedMessage = await LanguageTranslate(reportCreationDto.ReportMessage, detectedLanguage);
                using (var httpClient = new HttpClient())
                {
                    // Complaint Prediction
                    var complaintPredictionRequestBody = new { text = translatedMessage };
                    var httpContent = new StringContent(JsonConvert.SerializeObject(complaintPredictionRequestBody), System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(apiUrl, httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseContent);
                        prediction = (string)jsonResponse["prediction"];

                        switch (prediction)
                        {
                            case "SystemError": type = ComplaintType.SystemError; break;
                            case "BillingError": type = ComplaintType.BillingError; break;
                            case "ServiceDelay": type = ComplaintType.ServiceDelay; break;
                            case "EquipmentIssue": type = ComplaintType.EquipmentIssue; break;
                            case "PolicyViolation": type = ComplaintType.PolicyViolation; break;
                            case "CustomerFeedback": type = ComplaintType.CustomerFeedback; break;
                            case "Other": type = ComplaintType.Other; break;
                            default: type = ComplaintType.Other; break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to call API. StatusCode: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new GeneralResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the report"
                };
            }

            var newReport = new Report
            {
                ReportType = type,
                ReportMessege = $"({detectedLanguage}){translatedMessage}",
                IsFixed = false,
                ReporterId = _decodeJwt.GetUserIdFromToken(token),
                Timestamp = DateTime.UtcNow.AddHours(3)
            };

            await _unitOfWork.Report.AddAsync(newReport);
            await _unitOfWork.SaveChangeAsync();

            return new GeneralResponse
            {
                Success = true,
                Message = "Report Created Successfully" + ", Report Type is " + prediction
            };
        }

        public IEnumerable<ReadReportDto> GetAllReports(int turn)
        {
            var result = _unitOfWork.Report.FindAll(r => !r.IsFixed && r.ReportReceiver == null).Select(r => new ReadReportDto
            {
                ReportId = r.ReportId,
                IsFixed = r.IsFixed,
                ReportType = r.ReportType,
                ReportMessage = r.ReportMessege,
                ReporterId = r.ReporterId,
                ReporterName = _userManager.FindByIdAsync(r.ReporterId).Result.FullName
        }).Skip((turn - 1) * 30).Take(30).ToList();
            return result;
        }

        public IEnumerable<GarageAdminsListDto> GetAllGarageadmin()
        {
            var result = _unitOfWork.GarageAdmin.Include(r => r.Garage).Select(r => new GarageAdminsListDto
            {
                AdminId = r.Id,
                GarageId = r.GarageId.ToString(),
                GarageName = r.Garage.garageName,
                Name = r.FullName
            }).ToList();
            return result;
        }

        public async Task<GeneralResponse> UpdateReportStatusAsync(int reportId, string token)//if customer service solved it
        {
            string? userid = _decodeJwt.GetUserIdFromToken(token);
            var user = await _userManager.FindByIdAsync(userid);
            var role = await _userManager.GetRolesAsync(user);
            var report = await _unitOfWork.Report.FindAsync(r => r.ReportId == reportId);
            var reporter = await _userManager.FindByIdAsync(report.ReporterId);
            var reportermail = reporter.Email;
            if (report == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Report Not Found"
                };
            }
            try
            {
                var history = await _unitOfWork.History.AddAsync(new History
                {
                    HistoryType = Common.Enum.HistroyType.Complaint,
                    Timestamp = DateTime.UtcNow.AddHours(3),
                });
                await _unitOfWork.SaveChangeAsync();

                report.ReportReceiver = userid;

                _unitOfWork.Report.Delete(report);
                await _unitOfWork.SaveChangeAsync();

                var complainthistory = new ComplaintHistory
                {
                    ComplaintReceiver = userid,
                    ComplainantType = role[0],
                    ComplaintMessage = report.ReportMessege,
                    SolvedTime = DateTime.UtcNow.AddHours(3),
                    ComplaintTime = report.Timestamp,
                    ComplainantId = report.ReporterId,
                    HistoryId = history.HistoryId,
                    ComplaintType = report.ReportType,
                };
                _unitOfWork.ComplaintHistory.Add(complainthistory);
                await _unitOfWork.SaveChangeAsync();
                string htmlFileName = "ComplaintSolvedEmailMessage.html";

                var filePath = Path.Combine(_env.WebRootPath, "HTMLs", htmlFileName);
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"{htmlFileName} file not found.", filePath);
                }
                var htmlContent = await File.ReadAllTextAsync(filePath);

                htmlContent = htmlContent.Replace("{complaint}", report.ReportMessege);
                List<string> mail = new List<string>();
                mail.Add(reportermail);
                bool sent = await _emailSender.SendEmailAsync(mail, "Your Complaint", htmlContent);
                return new GeneralResponse
                {
                    Success = true,
                    Message = "Report Updated Successfully"
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "An error occurred while updating the report."
                };
            }
        }

        public async Task<GeneralResponse> ForwardReport(int reportId, string token, string reportReceiverId)
        {
            string? customerserviceid = _decodeJwt.GetUserIdFromToken(token);
            var report = await _unitOfWork.Report.FindAsync(r => r.ReportId == reportId);
            if (report == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Report Not Found"
                };
            }
            report.CustomerServiceId = customerserviceid;
            report.ReportReceiver = reportReceiverId;
            _unitOfWork.Report.Update(report);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = "Report Forwarded Successfully"
            };
        }

        public IEnumerable<ReadReportDto> GetReportsBasedOnRoleAsync(string? token)
        {
            string? receiverId = _decodeJwt.GetUserIdFromToken(token);
            var result = _unitOfWork.Report.FindAll(r => !r.IsFixed && r.ReportReceiver == receiverId).Select(r => new ReadReportDto
            {
                ReportId = r.ReportId,
                IsFixed = r.IsFixed,
                ReportType = r.ReportType,
                ReportMessage = r.ReportMessege,
                ReporterId = r.ReporterId,
                ReporterName = _userManager.FindByIdAsync(r.ReporterId).Result.FullName
            }).ToList();
            return result;
        }

    }
}