using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Rakna.BAL.DTO;
using Rakna.BAL.DTO.DriverDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.TechnicalSupportDtos;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.DAL;
using Rakna.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Rakna.Common.Enum;
using Rakna.BAL.DTO.BulkMailDto;
using Rakna.BAL.DTO.Statistics;
using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Serilog;
using Rakna.BAL.DTO.AIDto;

namespace Rakna.BAL.Service
{
    public class TechnicalSupportService : ITechnicalSupportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSendService _EmailSendService;
        private readonly IDecodeJwt _decodeJwt;
        private SpHelper sphelper = new SpHelper();

        public TechnicalSupportService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSendService EmailSendService, IDecodeJwt decodeJwt)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _EmailSendService = EmailSendService;
            _decodeJwt = decodeJwt;
        }

        public async Task<GeneralResponse> AddGarage(GarageDto garageDto)
        {
            string errors = sphelper.isValidGarage(garageDto);
            if (errors.Length > 0)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = errors
                };
            }
            Garage garage = new Garage
            {
                garageName = garageDto.GarageName,
                AvailableParkingSlots = garageDto.TotalSpaces,
                HourPrice = garageDto.HourPrice,
                street = garageDto.street,
                TotalParkingSlots = garageDto.TotalSpaces,
                city = garageDto.city,
                Latitude = garageDto.Latitude,
                Longitude = garageDto.Longitude,
            };
            _unitOfWork.Garage.Add(garage);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = $"Garage with name : {garageDto.GarageName}  added successfully"
            };
        }

        public async Task<GeneralResponse> DeleteGarage(int id)
        {
            var garages = await _unitOfWork.Garage.FindAsync(i => i.GarageId == id);
            if (garages == null)
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Garage not found"
                };
            _unitOfWork.Garage.Delete(garages);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = "Garage deleted successfully"
            };
        }

        public async Task<IEnumerable<DriverDto>> GetAllDriverID()
        {
            var driver = _unitOfWork.Driver.GetAll().Select(p => new DriverDto
            {
                Id = p.Id,
                NationalId = p.NationalNumber,
                Name = p.FullName,
                Email = p.Email,
                Phone = p.PhoneNumber,
            });
            return driver;
        }


        public async Task<ICollection<GetGarageDto>> GetAllGarages()
        {
            // Fetch all garages
            var allGarages = await _unitOfWork.Garage
                .AsNoTracking()
                .Select(g => new
                {
                    g.GarageId,
                    g.Longitude,
                    g.Latitude,
                    g.street,
                    g.city,
                    g.HourPrice,
                    g.AvailableParkingSlots,
                    g.TotalParkingSlots,
                    g.garageName
                }).ToListAsync();

            var garagesWithAdminIds = await _unitOfWork.GarageAdmin
                .AsNoTracking()
                .Select(ga => ga.GarageId)
                .ToListAsync();

            var garagesWithAdminStatus = allGarages.Select(g => new GetGarageDto
            {
                GarageId = g.GarageId,
                longitude = g.Longitude,
                latitude = g.Latitude,
                street = g.street,
                city = g.city,
                HourPrice = g.HourPrice,
                AvailableSpaces = g.AvailableParkingSlots,
                TotalSpaces = g.TotalParkingSlots,
                GarageName = g.garageName,
                HasAdmin = garagesWithAdminIds.Contains(g.GarageId)
            }).ToList();

            return garagesWithAdminStatus;
        }


        public async Task<GeneralResponse> RegisterUser(AddUserDto model)
        {
            string usernameErrors = sphelper.isValidUsername(model.UserName);
            string GenPassword = sphelper.GeneratePassword();

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new GeneralResponse { Message = "Email is already registered!", };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new GeneralResponse { Message = "Username is already used !" };

            if (model.NationalId.Length != 14 || !sphelper.isStringNumeric(model.NationalId))
                return new GeneralResponse { Message = "National ID is not Valid , try again !" };

            if (!sphelper.isValidEmail(model.Email))
                return new GeneralResponse { Message = "Email is not valid !" };

            if (usernameErrors.Length > 0)
                return new GeneralResponse { Message = $"{usernameErrors}" };

            IdentityResult? signUpResult = null;

            if (model.Role == UserRole.garageadmin)
            {
                GarageAdmin garageAdmin = new GarageAdmin
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    AdminNationalID = model.NationalId,
                    GarageId = model.GarageId.Value
                };

                var IsGarageAdminExist = await _unitOfWork.GarageAdmin.FindAsync(e => e.GarageId == model.GarageId);
                if (IsGarageAdminExist != null)
                {
                    return new GeneralResponse
                    {
                        Success = false,
                        Message = "This garage already has an admin!"
                    };
                }
                signUpResult = await _userManager.CreateAsync(garageAdmin, GenPassword);

                if (signUpResult == null || !signUpResult.Succeeded)
                {
                    return new GeneralResponse
                    {
                        Message = string.Join(" | ", signUpResult.Errors.Select(e => e.Description)),
                    };
                }
                await _userManager.AddToRoleAsync(garageAdmin, "garageadmin");
                await _userManager.UpdateAsync(garageAdmin);
                var datasend = await _EmailSendService.SendGarageAdminCreationData(model, GenPassword);
                if (!datasend.Success)
                {
                    await _userManager.DeleteAsync(garageAdmin);
                    return new GeneralResponse
                    {
                        Message = "Email not registered, Couldn't send profile information!",
                    };
                }

                var confirmtoken = await _userManager.GenerateEmailConfirmationTokenAsync(garageAdmin);
                var otpresult = await _EmailSendService.RegisterOTP(garageAdmin.Id, confirmtoken);

                if (!otpresult.Success)
                {
                    await _userManager.DeleteAsync(garageAdmin);
                    return new GeneralResponse
                    {
                        Message = "Email not registered!, Couldn't do the OTP thing",
                    };
                }
            }
            else if (model.Role == UserRole.customerservice)
            {
                CustomerService customerService = new CustomerService
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    NationalId = model.NationalId,
                    Salary = model.Salary.Value, // Assuming salary is mandatory for customer service
                };
                signUpResult = await _userManager.CreateAsync(customerService, GenPassword);

                if (signUpResult == null || !signUpResult.Succeeded)
                {
                    return new GeneralResponse
                    {
                        Message = string.Join(" | ", signUpResult.Errors.Select(e => e.Description)),
                    };
                }
                await _userManager.AddToRoleAsync(customerService, "customerservice");
                await _userManager.UpdateAsync(customerService);

                var datasend = await _EmailSendService.SendCustomerServiceCreationData(model, GenPassword);
                if (!datasend.Success)
                {
                    await _userManager.DeleteAsync(customerService);
                    return new GeneralResponse
                    {
                        Message = "Email not registered, Couldn't send profile information!",
                    };
                }

                var confirmtoken = await _userManager.GenerateEmailConfirmationTokenAsync(customerService);
                var otpresult = await _EmailSendService.RegisterOTP(customerService.Id, confirmtoken);
                if (!otpresult.Success)
                {
                    await _userManager.DeleteAsync(customerService);
                    return new GeneralResponse
                    {
                        Message = "Email not registered!",
                    };
                }
            }
            else
            {
                return new GeneralResponse { Message = "Invalid Role" };
            }

            return new GeneralResponse
            {
                Success = true,
                Message = "User Created Successfully"
            };
        }

        public async Task<GeneralResponse> EditStaffBasedOnRole(string id, EditUserDto model) // Edit GarageAdmin, CustomerService, or both
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            // Check user roles and cast to the correct type
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("garageadmin"))
            {
                var staff = await _unitOfWork.GarageAdmin.FindAsync(i => i.Id == id);
                // Update common properties
                staff.UserName = model.UserName;
                staff.Email = model.Email;
                staff.PhoneNumber = model.PhoneNumber;
                staff.FullName = model.FullName;
                staff.AdminNationalID = model.NationalId;
                staff.GarageId = model.GarageId.Value; // Assuming GarageAdmin has a GarageId
                _unitOfWork.GarageAdmin.Update(staff);
            }
            else if (roles.Contains("customerservice"))
            {
                var staff = await _unitOfWork.CustomerService.FindAsync(i => i.Id == id);
                // Update common properties and salary
                staff.UserName = model.UserName;
                staff.Email = model.Email;
                staff.PhoneNumber = model.PhoneNumber;
                staff.FullName = model.FullName;
                staff.NationalId = model.NationalId;
                staff.Salary = model.Salary.Value; // Assuming CustomerService has a Salary
                _unitOfWork.CustomerService.Update(staff);
            }
            else
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Role not supported for editing"
                };
            }

            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = "User updated successfully"
            };
        }

        public async Task<GeneralResponse> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            await _userManager.DeleteAsync(user);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = "User deleted successfully"
            };
        }
        public async Task<IEnumerable<ListUserDto>> GetAllUsers()
        {
            var userList = new List<ListUserDto>();

            var garageAdmins = await _unitOfWork.GarageAdmin.GetAllAsync();
            var customerServices = await _unitOfWork.CustomerService.GetAllAsync();

            foreach (var garageAdmin in garageAdmins)
            {
                var user = await _userManager.FindByIdAsync(garageAdmin.Id);
                if (user != null)
                {
                    userList.Add(new ListUserDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                        Email = user.Email,
                        GarageId = garageAdmin.GarageId,
                        Role = UserRole.garageadmin,
                        NationalId = garageAdmin.AdminNationalID
                    });
                }
            }

            // Iterate over CustomerServices
            foreach (var customerService in customerServices)
            {
                var user = await _userManager.FindByIdAsync(customerService.Id);
                if (user != null)
                {
                    userList.Add(new ListUserDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                        Email = user.Email,
                        Salary = customerService.Salary,
                        Role = UserRole.customerservice,
                        NationalId = customerService.NationalId
                    });
                }
            }

            return userList;
        }


        public async Task<GeneralResponse> UpdateGarage(int garageId, GarageDto garageDto)
        {
            var existingGarage = await _unitOfWork.Garage.FindAsync(i => i.GarageId == garageId);
            if (existingGarage == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Garage not found"
                };
            }

            existingGarage.HourPrice = garageDto.HourPrice;
            existingGarage.street = garageDto.street;
            existingGarage.TotalParkingSlots = garageDto.TotalSpaces;
            existingGarage.city = garageDto.city;
            existingGarage.Latitude = garageDto.Latitude;
            existingGarage.Longitude = garageDto.Longitude;
            existingGarage.garageName = garageDto.GarageName;
            _unitOfWork.Garage.Update(existingGarage);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = $"Garage with Name {garageDto.GarageName} updated successfulyy"
            };
        }

        public async Task<IEnumerable<TechReadingReportDto>> GetAllReports()
        {
            var reports = await _unitOfWork.Report.GetAllAsync();
            return reports.Select(report => new TechReadingReportDto
            {
                ReportId = report.ReportId,
                ReportType = report.ReportType,
                ReportMessage = report.ReportMessege
            });
        }

        public async Task<ICollection<BulkListDto>> GetAllUsersWithRolesAsync(string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var user = await _userManager.FindByIdAsync(userId);

            var querydriver = await _userManager.GetUsersInRoleAsync(UserRole.driver.ToString().ToLower());
            var querycustomer = await _userManager.GetUsersInRoleAsync(UserRole.customerservice.ToString().ToLower());
            var queryadmin = await _userManager.GetUsersInRoleAsync(UserRole.garageadmin.ToString().ToLower());

            List<BulkListDto> list = new List<BulkListDto>();
            list.AddRange(querydriver.Select(e => new BulkListDto
            {
                Email = e.Email,
                Name = e.FullName,
                RoleType = UserRole.driver
            }));
            list.AddRange(querycustomer.Select(e => new BulkListDto
            {
                Email = e.Email,
                Name = e.FullName,
                RoleType = UserRole.customerservice
            }));
            list.AddRange(queryadmin.Select(e => new BulkListDto
            {
                Email = e.Email,
                Name = e.FullName,
                RoleType = UserRole.garageadmin
            }));

            return list;
        }

        public async Task<IEnumerable<GarageStatisticsResponseDto>> GetAllGarageStatistics(StatisticsRequestDto request)
        {
            try
            {
                var garages = await _unitOfWork.Garage.GetAllAsync(); // Get all garages
                if (!garages.Any())
                {
                    Log.Information("No garages found.");
                    return Enumerable.Empty<GarageStatisticsResponseDto>(); // Return empty if no garages
                }

                List<GarageStatisticsResponseDto> allGarageStatistics = new List<GarageStatisticsResponseDto>();

                foreach (var garage in garages)
                {
                    var garageId = garage.GarageId;
                    var garageAdmin = await _unitOfWork.GarageAdmin.FindAsync(g => g.GarageId == garageId);
                    if (garageAdmin == null)
                    {
                        Log.Information($"No admin found for garage ID {garageId}. Skipping this garage.");
                        continue; // Skip this garage if no admin found
                    }

                    var garagestats = await CalculateGarageStatisticsAsyncFast(request,garage);
                    allGarageStatistics.Add(garagestats);
                }

                return allGarageStatistics;
            }
            catch (Exception ex)
            {
                Log.Information($"An error occurred in GetAllGarageStatistics: {ex.Message}");
                throw; // Rethrowing the exception is usually not best practice unless you handle it specifically higher up.
            }
        }
        public async Task<GarageStatisticsResponseDto> CalculateGarageStatisticsAsyncFast(StatisticsRequestDto request, Garage garage)
        {
            var garageId = garage.GarageId;
            var sessions = _unitOfWork.ParkingSessionHistory
                .FindAll(p => p.GarageId == garageId && p.EnterTime >= request.StartDate && p.LeaveTime <= request.EndDate).ToList();

            var reservations = _unitOfWork.ReservationHistory
                .FindAll(r => r.GarageId == garageId && r.ReservationTime >= request.StartDate && r.ReservationTime <= request.EndDate).ToList();

            var salaries = _unitOfWork.StaffSalaryHistory
                .FindAll(s => s.Employee.GarageId == garageId && s.CollectTime >= request.StartDate && s.CollectTime <= request.EndDate).ToList();

            var complaints = _unitOfWork.ComplaintHistory
                .FindAll(c => c.ComplaintReceiver == garage.GarageAdmin.Id && c.ComplaintTime >= request.StartDate && c.ComplaintTime <= request.EndDate).ToList();

            var complaints_nonsolved = _unitOfWork.Report
            .FindAll(c => c.ReportReceiver == garage.GarageAdmin.Id && c.Timestamp >= request.StartDate && c.Timestamp <= request.EndDate).ToList();

            var statistics = new GarageStatisticsResponseDto
            {
                GarageId = garageId.ToString(),
                AverageParkingDuration = new AverageParkingDurationResponseDto
                {
                    AverageDuration = sessions.IsNullOrEmpty() ? TimeSpan.FromSeconds(0) : TimeSpan.FromSeconds(sessions.Average(s => (s.LeaveTime - s.EnterTime).TotalSeconds))
                },
                TotalRevenue = new TotalRevenueResponseDto
                {
                    SumRequiredPayments = sessions.Sum(s => s.RequiredPayment),
                    SumActualPayments = sessions.Sum(s => s.ActualPayment),
                    ProfitFromOverpay = sessions.Where(s => s.ActualPayment > s.RequiredPayment).Sum(s => s.ActualPayment - s.RequiredPayment),
                    NumberOfCardPayments = sessions.Count(s => s.PaymentType == PaymentMethod.Card),
                    NumberOfCashPayments = sessions.Count(s => s.PaymentType == PaymentMethod.Cash),
                    NumberOfMobilePayments = sessions.Count(s => s.PaymentType == PaymentMethod.Mobile)
                },
                TotalReservations = new TotalReservationsResponseDto
                {
                    NumberOfReservations = reservations.Count,
                    SumReservationMoney = (garage.HourPrice * reservations.Count(r => r.UsedOrNot)), // Assuming a Cost field exists
                    ReservationThatIsActuallyUsed = reservations.Count(r => r.UsedOrNot),
                    PeakHoursOfTheDay = reservations.GroupBy(r => r.ReservationTime.Hour)
                                            .OrderByDescending(g => g.Count())
                                            .Take(3)
                                            .Select(g => g.Key).ToList()
                },
                TotalSalaryPaid = new TotalSalaryPaidResponseDto
                {
                    TotalSalaryPaid = salaries.Sum(s => s.Amount)
                },
                StaffActivityRating = new StaffActivityRatingResponseDto
                {
                    StaffActivities = sessions.GroupBy(p => p.StaffId)
                                              .Select(group => new StaffActivityRatingDto
                                              {
                                                  StaffId = group.Key,
                                                  StaffName = _unitOfWork.Employee.Find(e => e.Id == group.First().StaffId).FullName,
                                                  NumberOfSessions = group.Count()
                                              }).ToList()
                },
                ComplaintsStatistics = new ComplaintsStatisticsResponseDto
                {
                    NumberOfComplaintsForwardedToGarage = complaints.Count,
                    ComplaintsByType = complaints.GroupBy(c => c.ComplaintType).ToDictionary(x => x.Key, x => x.Count()),
                    AverageResolutionTime = TimeSpan.FromSeconds(complaints
                .Where(c => c != null && c.SolvedTime > c.ComplaintTime)
                .DefaultIfEmpty()
                .Average(c => (c?.SolvedTime - c?.ComplaintTime)?.TotalSeconds ?? 0))
                },
                ComplaintsStatistics_nonsolved = new ComplaintsStatisticsResponseDto
                {
                    NumberOfComplaintsForwardedToGarage = complaints_nonsolved.Count,
                    ComplaintsByType = complaints_nonsolved.GroupBy(c => c.ReportType).ToDictionary(x => x.Key, x => x.Count()),
                    AverageResolutionTime = TimeSpan.Zero
                },
                PeakParkingHours = new PeakParkingHoursDto
                {
                    PeakHoursOfTheDay = sessions.GroupBy(p => p.EnterTime.Hour)
                                                .OrderByDescending(g => g.Count())
                                                .Take(3)
                                                .Select(g => g.Key).ToList()
                },
                ReservedVsNonReservedParkingUsage = new ReservedVsNonReservedParkingUsageDto
                {
                    ReservedCount = sessions.Count(p => p.reserved),
                    NonReservedCount = sessions.Count(p => !p.reserved)
                }
            };

            return statistics;
        }
        public async Task<ResponseAiConfidence> GetConfidence()
        {
            var confidence = await Task.Run(() => _unitOfWork.PlateRecognitionHistory.GetAll().ToList());
            var letters = confidence.Average(s => s.LettersConfidence);
            var objects = confidence.Average(s => s.objectConfidence);
            return new ResponseAiConfidence
            {
                CharacterConfidence = (letters * 100).ToString("0.00") + "%",
                ObjectConfidence = (objects * 100).ToString("0.00") + "%"
            };
        }

    }
}