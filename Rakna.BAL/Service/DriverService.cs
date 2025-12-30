using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Rakna.BAL.DTO;
using Rakna.BAL.DTO.DriverDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.HistoryDto;
using Rakna.BAL.DTO.ReportDto;
using Rakna.BAL.DTO.UpdateDriverDto;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.DAL;
using Rakna.DAL.Models;
using Rakna.DAL.Models.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Service
{
    public class DriverService : IDriverService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDecodeJwt _decodeJwt;

        private SpHelper spvalidate = new SpHelper();

        public DriverService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IDecodeJwt decodeJwt)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _decodeJwt = decodeJwt;
        }

        public async Task<GeneralResponse> AddVehicle(VehicleDto addVehicleDto, string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            addVehicleDto.VehicleNumber = addVehicleDto.VehicleNumber.Replace(" ", "");
            addVehicleDto.VehicleLetter = addVehicleDto.VehicleLetter.Replace(" ", "");
            if (!spvalidate.isValidPlate(addVehicleDto.VehicleLetter, addVehicleDto.VehicleNumber))
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Invalid Plate"
                };
            }
            var driver = _unitOfWork.Driver.Find(g => g.Id == userId);
            if (driver is null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "There is no driver related with this id"
                };
            }
            if (driver.NationalNumber == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "You should Add National Id before adding your car"
                };
            }
            var ExixtedPlate = _unitOfWork.Vehicle.
         FindAll(g => g.PlateLetters == addVehicleDto.VehicleLetter &&
            g.PlateNumbers == addVehicleDto.VehicleNumber);
            if (ExixtedPlate.Count() > 0)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "this plate is already registered"
                };
            }
            Vehicle vehicle = new Vehicle()
            {
                PlateNumbers = addVehicleDto.VehicleNumber,
                PlateLetters = addVehicleDto.VehicleLetter,
                DriverId = userId
            };
            _unitOfWork.Vehicle.Add(vehicle);
            _unitOfWork.SaveChange();
            return new GeneralResponse
            {
                Success = true,
                Message = $"Car {addVehicleDto.VehicleLetter} {addVehicleDto.VehicleNumber} is successfully added"
            };
        }
        public async Task<GeneralResponse> EditVehicle(int VehicleId , VehicleDto vehicle)
        {
            var Vehicle = await _unitOfWork.Vehicle.FindAsync(v => v.VehicleId == VehicleId);
            if (Vehicle == null)
            {
                return new GeneralResponse { Message = " Vehicle not found" };
            }
            Vehicle.PlateLetters= vehicle.VehicleLetter;
            Vehicle.PlateNumbers =vehicle.VehicleNumber;
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse() {
                Success = true,
                 Message = $"Vehicle updated successfully , plate : {Vehicle.PlateNumbers} | {Vehicle.PlateLetters}" 
            };
         
        }

        public async Task<GeneralResponse> DeleteVehicle(int VehicleId)
        {
            var Vehicle =  await _unitOfWork.Vehicle.FindAsync(v => v.VehicleId == VehicleId);
           if(Vehicle == null)
           {
                return new GeneralResponse { Message = "Vehicle not found" };
           }
            _unitOfWork.Vehicle.Delete(Vehicle);
            await  _unitOfWork.SaveChangeAsync();
            return new GeneralResponse() { Success = true, Message = "Vehicle deltet successfully" };
        }
        public async Task<IEnumerable<GetVehcileDto>> GetAllVehicle(string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
           
           var Vehcile =   _unitOfWork.Vehicle.FindAll(v => v.DriverId == userId).Select( d => new  GetVehcileDto
            {
                Id = d.VehicleId,
                PlateLetter = d.PlateLetters,
                PlateNumber = d.PlateNumbers,
            }).ToList();
            if (Vehcile.IsNullOrEmpty())
            {
                return Enumerable.Empty<GetVehcileDto>();
            }
            return Vehcile;
        }

        public async Task<DriverProfileDto> DriverProfileDetails(string token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var user = await _unitOfWork.Driver.FindAsync(g => g.Id == userId);
            if (user == null)
            {
                return null;
            }
            DriverProfileDto driverProfileDto = new DriverProfileDto()
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                NationalId = user.NationalNumber
            };
            return driverProfileDto;
        }

        public async Task<GeneralResponse> Reservation(DriverReservationDto reservationDto, string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);

            var CheckForExistingReservation =  _unitOfWork.Garage.AsNoTracking()
                 .Include(r => r.Reservations)
                 .ThenInclude(d => d.Driver)
                 .Where(p => p.GarageId == reservationDto.GarageId);
            var garageName = _unitOfWork.Garage.Find(g => g.GarageId == reservationDto.GarageId);
            if(garageName == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Garage not found!"
                };
            }
            if (CheckForExistingReservation.Any(p =>
            p.Reservations.Any(existingReservation => existingReservation.DriverID == userId &&
        ((existingReservation.DateTime >= reservationDto.dateTime.AddHours(-1) && existingReservation.DateTime <= reservationDto.dateTime) ||
        (existingReservation.DateTime >= reservationDto.dateTime && existingReservation.DateTime <= reservationDto.dateTime.AddHours(1))))))
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "You can't have more than one reservation at the same hour!"
                };
            }
            if (reservationDto.dateTime < DateTime.UtcNow.AddHours(3))
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Reservation time cannot be in the past!"
                };
            }

            Reservation reservation = new Reservation
            {
                DateTime = reservationDto.dateTime,
                GarageId = reservationDto.GarageId,
                DriverID = userId
            };
            await _unitOfWork.Reservation.AddAsync(reservation);
            _unitOfWork.SaveChange();

            return new GeneralResponse
            {
                Success = true,
                Message = $"Your Reservation at Date : {reservationDto.dateTime} in {garageName.garageName} Garage  is succsessfully done"
            };
        }

        public async Task<IEnumerable<ReservationDetailsDto>> ReservationDetails(string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);

            var reservations = await _unitOfWork.Reservation.AsNoTracking()
               .Include(d => d.Driver).Where(d => d.DriverID == userId).Select(p => new ReservationDetailsDto
               {
                   ReservationID = p.ReservationId,
                   GarageLocation = p.Garage.city,
                   ReservationCost = (double)p.Garage.HourPrice,
                   DriverName = p.Driver.FullName,
                   ReservationDate = p.DateTime,
               }).ToListAsync();
            return reservations;
        }

        public IEnumerable<GetDriverReportDto> UnsolvedDriversReport(string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var report = _unitOfWork.Report.FindAll(r => r.ReporterId == userId).Select(p => new GetDriverReportDto
            {
                IsFixed = p.IsFixed,
                ReportMessage = p.ReportMessege,
                ReportType = p.ReportType,
            });
            if (report == null)
            {
                return Enumerable.Empty<GetDriverReportDto>();
            }
            return report;
        }

        public IEnumerable<ComplaintsHistoryDto> solvedDriversReport(string? token)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var report = _unitOfWork.ComplaintHistory.FindAll(f => f.ComplainantId == userId)
                .Select(p => new ComplaintsHistoryDto
                {
                    ComplaintMessage = p.ComplaintMessage,
                    ComplaintType = p.ComplaintType,
                    ComplaintTime = p.ComplaintTime,
                    SolvedTime = p.ComplaintTime,
                });
            if (report == null)
            {
                return Enumerable.Empty<ComplaintsHistoryDto>();
            }
            return report;
        }

        public async Task<GeneralResponse> UpdateDriverDetails(string token, UpdateDriverDetailsDto updateDto)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var driver = await _unitOfWork.Driver.FindAsync(g => g.Id == userId);

            if (driver == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Driver not found."
                };
            }

            // Update the driver's properties
            driver.FullName = updateDto.FullName;
            driver.PhoneNumber = updateDto.PhoneNumber;
            driver.NationalNumber = updateDto.NationalId;

            var updateResult = await _userManager.UpdateAsync(driver);
            if (!updateResult.Succeeded)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = string.Join("; ", updateResult.Errors.Select(e => e.Description))
                };
            }
            return new GeneralResponse
            {
                Success = true,
                Message = "Driver updated successfully."
            };
        }

        public async Task<GeneralResponse> UpdateDriverPassword(string token, UpdateDriverPasswordDto updateDto)
        {
            string passwordErrors = spvalidate.isValidPassword(updateDto.NewPassword);
            if (passwordErrors.Length > 0)
                return new GeneralResponse { Message = $"{passwordErrors}" };

            if (updateDto.CurrentPassword == updateDto.NewPassword)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "New password cannot be the same as the current password."
                };
            }

            var userId = _decodeJwt.GetUserIdFromToken(token);
            var driver = await _unitOfWork.Driver.FindAsync(g => g.Id == userId);

            if (driver == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Driver not found."
                };
            }

            var ChangePassword = await _userManager.ChangePasswordAsync(driver, updateDto.CurrentPassword, updateDto.NewPassword);

            if (!ChangePassword.Succeeded)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = string.Join("; ", ChangePassword.Errors.Select(e => e.Description))
                };
            }

            return new GeneralResponse
            {
                Success = true,
                Message = $"Password for {driver.FullName} updated successfully."
            };
        }

        public async Task<IEnumerable<RealTimeParkingSessionDto>> RealTimeParkingSessionsDetails(string? token)
        {
            var userId =  _decodeJwt.GetUserIdFromToken(token);
            var driver = await _unitOfWork.Driver.FindAsync(e=>e.Id==userId);
            var parkingSessionsWithDrivers = _unitOfWork.ParkingSession
                .Include(ps => ps.Vehicle) 
                .Where(ps => ps.Vehicle != null && ps.Vehicle.DriverId == userId)
                .Select(ps=>new RealTimeParkingSessionDto
                {
                    DriverName = ps.Vehicle.Driver.FullName,
                    GarageID = ps.GarageID.ToString(),
                    GarageCity = ps.Garage.city.ToString(),
                    GarageStreet = ps.Garage.street.ToString(),
                    SessionStart = ps.StartTime.ToString(),
                    SessionTime = (DateTime.UtcNow.AddHours(3)-ps.StartTime),
                    SessionPrice = (ps.Garage.HourPrice/3600)* (DateTime.UtcNow.AddHours(3) - ps.StartTime).TotalSeconds,
                    carplate = ps.PlateLetters+" "+ps.PlateNumbers
                });
            return parkingSessionsWithDrivers;
        }

        public async Task<GeneralResponse> CancelReservation(int ResevationId)
        {
            var Reservation = _unitOfWork.Reservation.GetById(ResevationId);
            if(Reservation == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "No Resevation found."
                };
            }
            _unitOfWork.Reservation.Delete(Reservation);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message="Reservation deleted successully"
            };
        }

        public async Task<GeneralResponse> EditReservation( string? token,int ResevationId, DriverReservationDto reservationDto)
        {
            var userId = _decodeJwt.GetUserIdFromToken(token);
            var CheckForExistingReservation = _unitOfWork.Garage.AsNoTracking()
                 .Include(r => r.Reservations)
                 .ThenInclude(d => d.Driver)
                 .Where(p => p.GarageId == reservationDto.GarageId);
            var garageName = _unitOfWork.Garage.Find(g => g.GarageId == reservationDto.GarageId);
            var nowPlus3Hours = DateTime.UtcNow.AddHours(3);

            if (CheckForExistingReservation.Any(p =>
                p.Reservations.Any(existingReservation =>
                    (nowPlus3Hours >= existingReservation.DateTime.AddMinutes(-30) && nowPlus3Hours <= existingReservation.DateTime.AddMinutes(30)))))
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "The time window has already started, you can't change the reservation!"
                };
            }
            if (reservationDto.dateTime < DateTime.UtcNow.AddHours(3))
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Reservation time cannot be in the past!"
                };
            }
            var Reservation = await _unitOfWork.Reservation.FindAsync(r => r.ReservationId == ResevationId);
            var grg = await _unitOfWork.Garage.GetByIdAsync(reservationDto.GarageId);
            if(grg == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "where garage?? uwu"
                };
            }
            Reservation.GarageId= reservationDto.GarageId;
            Reservation.DateTime=reservationDto.dateTime;
             _unitOfWork.Reservation.Update(Reservation);
            await _unitOfWork.SaveChangeAsync();
            return new GeneralResponse
            {
                Success = true,
                Message = "Reservation updated successfully"
            };
        }
    }
}