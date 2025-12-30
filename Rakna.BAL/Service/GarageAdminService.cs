using Microsoft.AspNetCore.Identity;
using Rakna.DAL.Models;
using Rakna.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rakna.BAL.Interface;
using Rakna.BAL.Helper;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.GarageAdminDtos;
using Rakna.DAL.DTO.GarageStaffDto;
using Microsoft.EntityFrameworkCore;
using Rakna.BAL.DTO;
using Rakna.DAL.Models.History;
using Rakna.Common.Enum;
using Newtonsoft.Json.Linq;
using Rakna.BAL.DTO.BulkMailDto;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Rakna.BAL.DTO.Statistics;
using Microsoft.IdentityModel.Tokens;

namespace Rakna.BAL.Service
{
    public class GarageAdminService : IGarageAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDecodeJwt _decodeJwt;
        private readonly IEmailSendService _EmailSendService;
        private SpHelper sphelper = new SpHelper();

        public GarageAdminService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IDecodeJwt decodeJwt, IEmailSendService EmailSendService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _decodeJwt = decodeJwt;
            _EmailSendService = EmailSendService;
        }

        public async Task<GeneralResponse> AddStaff(AddStaffDTO Model, string? token)
        {
            string? AdminId = _decodeJwt.GetUserIdFromToken(token);
            var garageId = _unitOfWork.Garage.Include(a => a.GarageAdmin)
                .Where(g => g.GarageAdmin.Id == AdminId)
                .Select(i => i.GarageId).ToList().FirstOrDefault();
            var garage = _unitOfWork.Garage.Find(g => g.GarageId == garageId);
            if (garage == null || garageId == null)
            {
                return new GeneralResponse { Message = "Garage not found" };
            }
            string GenPassword = sphelper.GeneratePassword();
            string passwordErrors = sphelper.isValidPassword(GenPassword);
            string usernameErrors = sphelper.isValidUsername(Model.UserName);

            if (await _userManager.FindByEmailAsync(Model.Email) is not null)
                return new GeneralResponse { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(Model.UserName) is not null)
                return new GeneralResponse { Message = "Username is already used !" };

            if (Model.NationalId.Length != 14 || !sphelper.isStringNumeric(Model.NationalId))
                return new GeneralResponse { Message = "National ID is not Valid , try again !" };

            if (!sphelper.isValidEmail(Model.Email))
                return new GeneralResponse { Message = "Email is not valid !" };

            if (usernameErrors.Length > 0)
                return new GeneralResponse { Message = $"{usernameErrors}" };

            IdentityResult? SignUpResult = null;
            Employee employee = new Employee()
            {
                UserName = Model.UserName,
                Email = Model.Email,
                PhoneNumber = Model.PhoneNumber,
                FullName = Model.name,
                NationalID = Model.NationalId,
                GarageId = garageId,
                Garage = garage,
                Salary = Model.salary,
                DateOfJoining = DateTime.UtcNow.AddHours(3),
            };
            SignUpResult = await _userManager.CreateAsync(employee, GenPassword);
            if (SignUpResult is null || !SignUpResult.Succeeded)
            {
                return new GeneralResponse
                {
                    Message = string.Join(" | ", SignUpResult.Errors.Select(e => e.Description)),
                };
            }
            await _userManager.AddToRoleAsync(employee, "garagestaff");

            var datasend = await _EmailSendService.SendStaffCreationData(Model, GenPassword);

            if (!datasend.Success)
            {
                await _userManager.DeleteAsync(employee);
                return new GeneralResponse
                {
                    Message = datasend.Message
                };
            }
            var confirmtoken = await _userManager.GenerateEmailConfirmationTokenAsync(employee);

            var otpresult = await _EmailSendService.RegisterOTP(employee.Id, confirmtoken);

            if (!otpresult.Success)
            {
                await _userManager.DeleteAsync(employee);
                return new GeneralResponse
                {
                    Message = "Email not registered!",
                };
            }
            return new GeneralResponse
            {
                Success = true,
                Message = "User Created Successfully"
            };
        }

        public async Task<GeneralResponse> DeleteStaff(string id)
        {
            var staff = await _unitOfWork.Employee.FindAsync(i => i.Id == id);
            if (staff == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Staff not found"
                };
            }
            _unitOfWork.Employee.Delete(staff);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = "Staff deleted successfully"
            };
        }

        public async Task<GeneralResponse> EditHourPrice(int newHourPrice, string? token)
        {
            string? AdminId = _decodeJwt.GetUserIdFromToken(token);
            var garageId = _unitOfWork.Garage.Include(a => a.GarageAdmin)
                .Where(g => g.GarageAdmin.Id == AdminId)
                .Select(i => i.GarageId).ToList().FirstOrDefault();
            var garage = await _unitOfWork.Garage.FindAsync(g => g.GarageId == garageId);
            if (garage == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Garage not found"
                };
            }

            garage.HourPrice = newHourPrice;
            _unitOfWork.Garage.Update(garage);
            await _unitOfWork.SaveChangeAsync();

            return new GeneralResponse
            {
                Success = true,
                Message = $"Hour price for garage updated successfully to {newHourPrice}"
            };
        }

        public async Task<GeneralResponse> EditStaff(string id, AddStaffDTO Model)
        {
            try
            {
                var existingstaff = await _unitOfWork.Employee.FindAsync(i => i.Id == id);
                if (existingstaff == null)
                {
                    return new GeneralResponse
                    {
                        Success = false,
                        Message = "Staff not found"
                    };
                }
                existingstaff.UserName = Model.UserName;
                existingstaff.Email = Model.Email;
                existingstaff.PhoneNumber = Model.PhoneNumber;
                existingstaff.FullName = Model.name;
                existingstaff.NationalID = Model.NationalId;
                existingstaff.Salary = Model.salary;
                _unitOfWork.Employee.Update(existingstaff);
                await _unitOfWork.SaveChangeAsync();
                return new GeneralResponse
                {
                    Success = true,
                    Message = $"Staff with Name {Model.name} updated successfully"
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

        /*        public async Task<GarageReportDto> GarageReport(string token, DateOnly date)
                {
                    string? AdminId =  _decodeJwt.GetUserIdFromToken(token);
                    var garageId = _unitOfWork.Garage.Include(a => a.GarageAdmin)
                        .Where(g => g.GarageAdmin.Id == AdminId)
                        .Select(i => i.GarageId).FirstOrDefault();

                    if (garageId == null)
                    {
                        return null;
                    }

                    var parkingSessions = _unitOfWork.ParkingSession.AsNoTracking().Include(g=>g.Vehicle).ThenInclude(d=>d.Driver)
                        .Where(ps => ps.GarageID == garageId && ps.EndTime.Date >= date.ToDateTime(TimeOnly.MinValue) && ps.IsActive==false)
                        .Select(ps => new ParkingSessionDetailsDto
                        {
                            DriverName = ps.Vehicle.Driver.FullName,
                            PlateLetters = ps.Vehicle.PlateLetters,
                            PlateNumbers = ps.Vehicle.PlateNumbers,
                            Bill = ps.Cost,
                            StartDate = ps.StartTime,
                            EndDate = ps.EndTime,
                            SessionDuration = (ps.EndTime - ps.StartTime).Minutes / 60.0,
                        }).ToList();

                    var totalMoney = parkingSessions.Sum(ps => ps.Bill);

                    var report = new GarageReportDto
                    {
                        TotalMoney = totalMoney,
                        parkingSessionDetailsDtos = parkingSessions
                    };

                    return report;
                }
        */

        public ICollection<StaffDto> getAllStaff(string token)
        {
            string? AdminId = _decodeJwt.GetUserIdFromToken(token);
            var garageId = _unitOfWork.Garage.Include(a => a.GarageAdmin)
                .Where(g => g.GarageAdmin.Id == AdminId)
                .Select(i => i.GarageId).ToList();

            var staffMembers = _unitOfWork.Employee.AsNoTracking().Where(e => e.GarageId == garageId.FirstOrDefault()).Select(staff => new StaffDto
            {
                Name = staff.FullName,
                NationalId = staff.NationalID,
                Email = staff.Email,
                salary = staff.Salary,
                phoneNumber = staff.PhoneNumber,
                userName = staff.UserName,
                Id = staff.Id
            }).ToList();

            return staffMembers;
        }

        public async Task<ICollection<GetAllStaffpaidSalaryDto>> GetAllStaffpaidSalary(string token)
        {
            string? AdminId = _decodeJwt.GetUserIdFromToken(token);
            var garageId = _unitOfWork.Garage.Include(a => a.GarageAdmin)
                .Where(g => g.GarageAdmin.Id == AdminId)
                .Select(i => i.GarageId).ToList();

            if (garageId.IsNullOrEmpty())
                return new List<GetAllStaffpaidSalaryDto>();

            var paidSalariesEmployeeIds = _unitOfWork.Employee.AsNoTracking()
                .Where(e => e.GarageId == garageId.FirstOrDefault())
                .Select(e => e.Id).ToList();

            if (paidSalariesEmployeeIds.IsNullOrEmpty())
                return new List<GetAllStaffpaidSalaryDto>();

            var paidSalariesHistory = _unitOfWork.StaffSalaryHistory
                .FindAll(s => paidSalariesEmployeeIds.Any(e => e == s.EmployeeId))
                .Select(s => new GetAllStaffpaidSalaryDto
                {
                    Name = s.StaffName,
                    Email = s.StaffEmail,
                    CollectedDate = s.CollectTime,
                    Amount = s.Amount
                }).ToList();

            return paidSalariesHistory;
        }

        public async Task<ICollection<GetAllStaffUnpaidSalaryDto>> GetAllStaffUnpaidSalary(string? token)
        {
            string? AdminId = _decodeJwt.GetUserIdFromToken(token);
            var garageId = _unitOfWork.Garage.Include(a => a.GarageAdmin)
                .Where(g => g.GarageAdmin.Id == AdminId)
                .Select(i => i.GarageId).ToList();

            if (garageId.IsNullOrEmpty())
                return null;

            var today = DateTime.Today;
            var firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);
            var paymentNotificationPeriodStart = firstDayOfNextMonth.AddDays(-5);

            if (today > paymentNotificationPeriodStart)
            {
                var paidEmployeeIdsThisMonth = await _unitOfWork.StaffSalaryHistory.AsNoTracking()
                    .Where(s => s.CollectTime >= firstDayOfThisMonth &&
                                s.CollectTime < firstDayOfNextMonth &&
                                s.Employee.GarageId == garageId.FirstOrDefault())
                    .Select(s => s.EmployeeId)
                    .Distinct()
                    .ToListAsync();

                var unpaidSalaries = await _unitOfWork.Employee.AsNoTracking()
                    .Where(e => e.GarageId == garageId.FirstOrDefault() && !paidEmployeeIdsThisMonth.Contains(e.Id))
                    .Select(e => new GetAllStaffUnpaidSalaryDto
                    {
                        StaffId = e.Id,
                        Name = e.FullName,
                        Email = e.Email,
                        Amount = e.Salary,
                        DaysUntilPayment = (firstDayOfNextMonth - DateTime.Today).Days
                    }).ToListAsync();

                return unpaidSalaries;
            }
            else
            {
                return new List<GetAllStaffUnpaidSalaryDto>();
            }
        }

        public async Task<StaffSalariesDto> GetAllStaffSalaries(string token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);

            if (admin == null)
                return new StaffSalariesDto
                {
                    Staffs = new List<StaffSalariesListDto>(),
                    DaysUntilPayment = 0
                };

            var today = DateTime.Today;
            var firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);

            var allEmployees = await _unitOfWork.Employee.GetAllAsync();
            var employeeSalaries = allEmployees
                .Where(e => e.GarageId == admin.GarageId)
                .Select(e => new StaffSalariesListDto
                {
                    StaffId = e.Id,
                    Name = e.FullName,
                    Email = e.Email,
                    Amount = (firstDayOfNextMonth - e.DateOfJoining).Days <= 29 ? e.Salary / 30 * (firstDayOfNextMonth - e.DateOfJoining).Days : e.Salary,
                    LastPayment = _unitOfWork.StaffSalaryHistory
                        .FindAll(s => s.EmployeeId == e.Id)
                        .OrderByDescending(s => s.CollectTime)
                        .Select(s => s.CollectTime)
                        .FirstOrDefault(),
                    isPaid = _unitOfWork.StaffSalaryHistory
                        .Any(s => s.EmployeeId == e.Id && s.CollectTime >= firstDayOfThisMonth),
                })
                .ToList();

            var resultdto = new StaffSalariesDto
            {
                Staffs = employeeSalaries,
                DaysUntilPayment = (firstDayOfNextMonth - DateTime.Today).Days
            };
            return resultdto;
        }

        public async Task<GeneralResponse> PaySalary(string id)
        {
            // Retrieve the employee based on id
            var employee = await _unitOfWork.Employee.FindAsync(e => e.Id == id);
            if (employee == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Employee not found."
                };
            }

            // Assuming the salary payment is for the current month
            var paymentDate = DateTime.UtcNow.AddHours(3); // or DateTime.UtcNow depending on your timezone handling
            var salaryDeservedDate = new DateTime(paymentDate.Year, paymentDate.Month, 1); // Assuming salary is deserved from the 1st day of the payment month

            var history = new History
            {
                Timestamp = DateTime.UtcNow.AddHours(3),
                HistoryType = HistroyType.Salary,
            };
            _unitOfWork.History.Add(history);
            await _unitOfWork.SaveChangeAsync();
            var today = DateTime.Today;
            var firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);
            // Create a new StaffSalaryHistory record
            var salaryHistory = new StaffSalaryHistory
            {
                EmployeeId = employee.Id,
                CollectTime = paymentDate,
                Amount = (firstDayOfNextMonth - employee.DateOfJoining).Days <= 29 ? employee.Salary / 30 * (firstDayOfNextMonth - employee.DateOfJoining).Days : employee.Salary,
                Employee = employee,
                StaffEmail = employee.Email,
                StaffName = employee.FullName,
                History = history,
                HistoryId = history.HistoryId
            };

            // Add the record to StaffSalaryHistory
            await _unitOfWork.StaffSalaryHistory.AddAsync(salaryHistory);
            await _unitOfWork.SaveChangeAsync();

            return new GeneralResponse
            {
                Success = true,
                Message = "Salary paid successfully."
            };
        }

        public async Task<ICollection<GettAllSessionDto>> gettAllSession(string token)
        {
            string? AdminId = _decodeJwt.GetUserIdFromToken(token);

            var garageId = _unitOfWork.Garage.Include(a => a.GarageAdmin)
                .Where(g => g.GarageAdmin.Id == AdminId)
                .Select(i => i.GarageId).ToList().FirstOrDefault();

            var AllSessions = await _unitOfWork.ParkingSession.AsNoTracking().Include(p => p.Vehicle)
                .ThenInclude(p => p.Driver).
                Where(g => g.GarageID == garageId).Select(p => new GettAllSessionDto
                {
                    StartDate = p.StartTime,
                    PlateNumbers = p.PlateNumbers,
                    PlateLetters = p.PlateLetters,
                    CurSessionDuration_Hours = DateTime.UtcNow.AddHours(3).Subtract(p.StartTime).TotalMinutes / 60.0,
                    CurrentBill = DateTime.UtcNow.AddHours(3).Subtract(p.StartTime).TotalMinutes * (p.Garage.HourPrice / 60),
                    DriverName = p.Vehicle.Driver.FullName,
                }).ToListAsync();
            return AllSessions;
        }

        public async Task<ICollection<BulkListDto>> getAllStaffbulk(string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);
            var querystaff = await _unitOfWork.Employee.FindAllAsync(e => e.GarageId == admin.GarageId);
            var staff = querystaff.Select(s => new BulkListDto
            {
                Email = s.Email,
                Name = s.FullName,
                RoleType = UserRole.garageadmin
            }).ToList();

            return staff;
        }

        public async Task<AverageParkingDurationResponseDto> CalculateAverageParkingDurationAsync(StatisticsRequestDto request, string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);

            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            var averageDuration = _unitOfWork.ParkingSessionHistory
                .FindAll(p => p.GarageId == admin.GarageId && p.EnterTime >= request.StartDate && p.LeaveTime <= request.EndDate)
                .Select(s => (s.LeaveTime - s.EnterTime).TotalSeconds)
                .DefaultIfEmpty(0)
                .Average();

            return new AverageParkingDurationResponseDto
            {
                AverageDuration = TimeSpan.FromSeconds(averageDuration),
            };
        }

        public async Task<TotalRevenueResponseDto> CalculateTotalRevenueAsync(StatisticsRequestDto request, string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);

            // Using a single query to calculate all required statistics
            var queryResult = _unitOfWork.ParkingSessionHistory
                .FindAll(p => p.GarageId == admin.GarageId && p.EnterTime >= request.StartDate && p.LeaveTime <= request.EndDate)
                .GroupBy(p => 1) // Dummy grouping to allow aggregation over the whole set
                .Select(g => new TotalRevenueResponseDto
                {
                    SumRequiredPayments = g.Sum(s => s.RequiredPayment),
                    SumActualPayments = g.Sum(s => s.ActualPayment),
                    ProfitFromOverpay = g.Where(s => s.ActualPayment > s.RequiredPayment).Sum(s => s.ActualPayment - s.RequiredPayment),
                    NumberOfCardPayments = g.Count(s => s.PaymentType == PaymentMethod.Card),
                    NumberOfCashPayments = g.Count(s => s.PaymentType == PaymentMethod.Cash),
                    NumberOfMobilePayments = g.Count(s => s.PaymentType == PaymentMethod.Mobile)
                }).FirstOrDefault();

            return queryResult ?? new TotalRevenueResponseDto
            {
                SumRequiredPayments = 0,
                SumActualPayments = 0,
                ProfitFromOverpay = 0,
                NumberOfCardPayments = 0,
                NumberOfCashPayments = 0,
                NumberOfMobilePayments = 0
            };
        }

        public async Task<TotalReservationsResponseDto> CalculateTotalReservationsAsync(StatisticsRequestDto request, string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);
            var garage = await _unitOfWork.Garage.FindAsync(e => e.GarageId == admin.GarageId);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            var reservationStats = _unitOfWork.ReservationHistory
                .FindAll(r => r.GarageId == admin.GarageId && r.ReservationTime >= request.StartDate && r.ReservationTime <= request.EndDate)
                .GroupBy(r => 1)
                .Select(group => new TotalReservationsResponseDto
                {
                    NumberOfReservations = group.Count(),
                    SumReservationMoney = (garage.HourPrice * group.Count()),
                    ReservationThatIsActuallyUsed = group.Count(r => r.UsedOrNot),
                    PeakHoursOfTheDay = group
                    .GroupBy(r => r.ReservationTime.Hour)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(3) // Get the top 3 hours
                    .ToList()
                }).FirstOrDefault();
            return reservationStats ?? new TotalReservationsResponseDto
            {
                NumberOfReservations = 0,
                SumReservationMoney = 0,
                ReservationThatIsActuallyUsed = 0,
                PeakHoursOfTheDay = new List<int>()
            };
        }

        public async Task<TotalSalaryPaidResponseDto> CalculateTotalSalaryPaidAsync(StatisticsRequestDto request, string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);

            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            var garageId = admin.GarageId; // Assuming GarageAdmin entity has GarageId property

            var totalSalaryPaid = _unitOfWork.StaffSalaryHistory
                .FindAll(s => s.Employee.GarageId == garageId && s.CollectTime >= request.StartDate && s.CollectTime <= request.EndDate)
                .Sum(s => s.Amount);

            return new TotalSalaryPaidResponseDto
            {
                TotalSalaryPaid = totalSalaryPaid
            };
        }

        public async Task<StaffActivityRatingResponseDto> CalculateStaffActivityRatingAsync(StatisticsRequestDto request, string token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            var garageId = admin.GarageId;

            var staffActivityRatings = _unitOfWork.ParkingSessionHistory
                .FindAll(p => p.GarageId == garageId && p.EnterTime >= request.StartDate && p.LeaveTime <= request.EndDate && p.StaffId != null)
                .GroupBy(p => p.StaffId)
                .Select(group => new StaffActivityRatingDto
                {
                    StaffId = group.Key,
                    StaffName = _unitOfWork.Employee.FindAll(e => e.Id == group.Key).FirstOrDefault().FullName ?? "NO NAME", // Assuming there's a navigation property to Staff
                    NumberOfSessions = group.Count()
                })
                .ToList();

            return new StaffActivityRatingResponseDto
            {
                StaffActivities = staffActivityRatings
            };
        }

        public async Task<ComplaintsStatisticsResponseDto> CalculateComplaintsStatisticsAsync(StatisticsRequestDto request, string token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            var garageId = admin.GarageId; // Assuming GarageAdmin entity has GarageId property

            // Retrieve all complaints forwarded to the garage within the date range
            var complaints = _unitOfWork.ComplaintHistory
                .FindAll(c => c.ComplaintReceiver == userId && c.ComplaintTime >= request.StartDate && c.ComplaintTime <= request.EndDate)
                .GroupBy(c => 1) // Group by a constant to aggregate the entire sequence
                .Select(g => new ComplaintsStatisticsResponseDto
                {
                    NumberOfComplaintsForwardedToGarage = g.Count(),
                    ComplaintsByType = g.GroupBy(c => c.ComplaintType).ToDictionary(x => x.Key, x => x.Count()),
                    AverageResolutionTime = TimeSpan.FromSeconds(g.Where(c => c.SolvedTime > DateTime.MinValue && c.ComplaintTime < c.SolvedTime)
                          .Select(c => (c.SolvedTime - c.ComplaintTime).TotalSeconds)
                          .DefaultIfEmpty(0)
                          .Average())
                }).FirstOrDefault();
            return complaints ?? new ComplaintsStatisticsResponseDto
            {
                NumberOfComplaintsForwardedToGarage = 0,
                ComplaintsByType = new Dictionary<ComplaintType, int>(),
                AverageResolutionTime = TimeSpan.FromSeconds(0)
            };
        }

        public async Task<PeakParkingHoursDto> GetPeakParkingHoursAsync(StatisticsRequestDto request, string token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            var garageId = admin.GarageId; // Assuming GarageAdmin entity has GarageId property

            var parkingSessions = _unitOfWork.ParkingSessionHistory
                    .FindAll(p => p.GarageId == admin.GarageId && p.EnterTime >= request.StartDate && p.EnterTime <= request.EndDate)
                    .AsEnumerable()  // AsEnumerable to switch to client-side execution for the next operations
                    .GroupBy(p => p.EnterTime.Hour) // Grouping by hour of the enter time
                    .Select(g => new { Hour = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count) // Ordering by the count to find peak hours
                    .Take(3) // Take the top 3 hours
                    .Select(g => g.Hour)
                    .ToList();

            return new PeakParkingHoursDto { PeakHoursOfTheDay = parkingSessions };
        }

        public async Task<ReservedVsNonReservedParkingUsageDto> GetReservedVsNonReservedParkingUsageAsync(StatisticsRequestDto request, string token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);

            var garageId = admin.GarageId;

            var parkingSessions = _unitOfWork.ParkingSessionHistory
                .FindAll(p => p.GarageId == garageId && p.EnterTime >= request.StartDate && p.EnterTime <= request.EndDate)
                .GroupBy(p => p.reserved)
                .Select(g => new { IsReserved = g.Key, Count = g.Count() })
                .ToList();

            var reservedCount = parkingSessions.FirstOrDefault(g => g.IsReserved == true)?.Count ?? 0;
            var nonReservedCount = parkingSessions.FirstOrDefault(g => g.IsReserved == false)?.Count ?? 0;

            return new ReservedVsNonReservedParkingUsageDto
            {
                ReservedCount = reservedCount,
                NonReservedCount = nonReservedCount
            };
        }

        public async Task<GarageStatisticsResponseDto> CalculateGarageStatisticsAsyncSlow(StatisticsRequestDto request, string? token)
        {
            var garageid = await _unitOfWork.Garage.FindAsync(g => g.GarageAdmin.Id == _decodeJwt.GetUserIdFromToken(token));

            var statistics = new GarageStatisticsResponseDto
            {
                GarageId = garageid.GarageId.ToString(),
                AverageParkingDuration = await CalculateAverageParkingDurationAsync(request, token),
                TotalRevenue = await CalculateTotalRevenueAsync(request, token),
                TotalReservations = await CalculateTotalReservationsAsync(request, token),
                TotalSalaryPaid = await CalculateTotalSalaryPaidAsync(request, token),
                StaffActivityRating = await CalculateStaffActivityRatingAsync(request, token),
                ComplaintsStatistics = await CalculateComplaintsStatisticsAsync(request, token),
                PeakParkingHours = await GetPeakParkingHoursAsync(request, token),
                ReservedVsNonReservedParkingUsage = await GetReservedVsNonReservedParkingUsageAsync(request, token)
            };
            return statistics;
        }

        public async Task<GarageStatisticsResponseDto> CalculateGarageStatisticsAsyncFast(StatisticsRequestDto request, string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var admin = await _unitOfWork.GarageAdmin.FindAsync(e => e.Id == userId);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            var garageId = admin.GarageId;
            var garage = await _unitOfWork.Garage.FindAsync(g => g.GarageId == garageId);
            var sessions = _unitOfWork.ParkingSessionHistory
                .FindAll(p => p.GarageId == garageId && p.EnterTime >= request.StartDate && p.LeaveTime <= request.EndDate).ToList();

            var reservations = _unitOfWork.ReservationHistory
                .FindAll(r => r.GarageId == garageId && r.ReservationTime >= request.StartDate && r.ReservationTime <= request.EndDate).ToList();

            var salaries = _unitOfWork.StaffSalaryHistory
                .FindAll(s => s.Employee.GarageId == garageId && s.CollectTime >= request.StartDate && s.CollectTime <= request.EndDate).ToList();

            var complaints = _unitOfWork.ComplaintHistory
                .FindAll(c => c.ComplaintReceiver == userId && c.ComplaintTime >= request.StartDate && c.ComplaintTime <= request.EndDate).ToList();

            var complaints_nonsolved = _unitOfWork.Report
                .FindAll(c => c.ReportReceiver == userId && c.Timestamp >= request.StartDate && c.Timestamp <= request.EndDate).ToList();

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
                                              .Select( group => new StaffActivityRatingDto
                                              {
                                                  StaffId = group.Key,
                                                  StaffName = _unitOfWork.Employee.Find(e=>e.Id == group.First().StaffId).FullName,
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

        public async Task<GarageDataDto> GetGarageData(string? token)
        {
            var AdminId = _decodeJwt.GetUserIdFromToken(token);
            var garage = await _unitOfWork.Garage.FindAsync(g => g.GarageAdmin.Id == AdminId);
            return new GarageDataDto
            {
                garageName = garage.garageName,
                HourPrice = garage.HourPrice
            };
        }
    }
}