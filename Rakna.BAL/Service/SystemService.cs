using Microsoft.AspNetCore.Identity;
using Rakna.BAL.DTO.AIDto;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.DAL.Models;
using Rakna.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rakna.DAL.Models.History;
using Rakna.Common.Enum;

namespace Rakna.BAL.Service
{
    public class SystemService : ISystemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSendService _EmailSendService;
        private readonly IDecodeJwt _decodeJwt;
        private SpHelper sphelper = new SpHelper();
        public SystemService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSendService EmailSendService, IDecodeJwt decodeJwt)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _EmailSendService = EmailSendService;
            _decodeJwt = decodeJwt;
        }
        public async Task<GeneralResponse> AddConfidenceAsync(List<DetectionResultDto> results,string token)
        {

            try
            {
            if (token != "Q0cbYVfQxDVye48AbdulsayedRaknaNigafa7XdaqTQejyH")
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Invalid Token"
                };
            }
            var history = await _unitOfWork.History.AddAsync(new History
            {
                HistoryType = HistroyType.PlateRecognition,
                Timestamp = DateTime.UtcNow.AddHours(3)
            });
            await _unitOfWork.SaveChangeAsync();
            var processedResults = results.Select(result => new
            {
                ObjectConfidence = result.ConfidenceObject,
                AverageCharacterConfidence = result.CharactersResult.Any() ? result.CharactersResult.Average(c => c.Confidence) : 0.0
            }).ToList();
            var value = new PlateRecognitionHistory
            {
                HistoryId = history.HistoryId,
                History = history,
                LettersConfidence = processedResults.FirstOrDefault().AverageCharacterConfidence,
                objectConfidence = processedResults.FirstOrDefault().ObjectConfidence,
            };
            await _unitOfWork.PlateRecognitionHistory.AddAsync(value);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = "Results added successfully"
            };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
            
    }
}
