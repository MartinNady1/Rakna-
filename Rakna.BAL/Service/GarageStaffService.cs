using Rakna.BAL.Interface;
using Rakna.DAL;
using Rakna.DAL.Models;
using Rakna.BAL.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rakna.BAL.DTO.GarageDto;
using Rakna.DAL.DTO.GarageStaffDto;
using Rakna.BAL.Helper;
using Rakna.BAL.Service;
using Rakna.DAL.Models.History;
using Rakna.Common.Enum;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.DriverDto;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class GarageStaffService : IGarageStaffService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDecodeJwt _decodeJwt;
    private SpHelper spvalidate = new SpHelper();

    public GarageStaffService(IUnitOfWork unitOfWork, IDecodeJwt decodeJwt)
    {
        _unitOfWork = unitOfWork;
        _decodeJwt = decodeJwt;
    }

    public async Task<IEnumerable<GettAllSessionDto>> GetCurrentParkingSessionsAsync(string? token)
    {
        var userId = _decodeJwt.GetUserIdFromToken(token);
        var user = await _unitOfWork.Employee.FindAsync(g => g.Id == userId);
        return await _unitOfWork.ParkingSession
            .AsNoTracking()
            .Include(p => p.Vehicle)
            .ThenInclude(v => v.Driver)
            .Where(g => g.GarageID == user.GarageId)
            .Select(p => new GettAllSessionDto
            {
                StartDate = p.StartTime,
                PlateNumbers = p.PlateNumbers,
                PlateLetters = p.PlateLetters,
                CurSessionDuration_Hours = DateTime.UtcNow.AddHours(3).Subtract(p.StartTime).TotalHours,
                CurrentBill = DateTime.UtcNow.AddHours(3).Subtract(p.StartTime).TotalMinutes * (p.Garage.HourPrice / 60),
                DriverName = p.Vehicle.Driver.FullName,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ReservationDetailsDto>> GetAllReservationsAsync(string token)
    {
        var userId = _decodeJwt.GetUserIdFromToken(token);
        var user = await _unitOfWork.Employee.FindAsync(g => g.Id == userId);
        return await _unitOfWork.Reservation
            .AsNoTracking()
            .Include(d => d.Driver)
            .Where(g => g.GarageId == user.GarageId)
            .Select(p => new ReservationDetailsDto
            {
                ReservationID = p.ReservationId,
                GarageLocation = p.Garage.city,
                ReservationCost = p.Garage.HourPrice,
                DriverName = p.Driver.FullName,
                NationalID = p.Driver.NationalNumber,
                ReservationDate = p.DateTime,
            })
            .ToListAsync();
    }

    public async Task<GeneralResponse> StartParkingSessionAsync(StartParkingSessionDto parkingSessionDto, string token)
    {
        parkingSessionDto.PlateNumbers = parkingSessionDto.PlateNumbers.Replace(" ", "");
        parkingSessionDto.PlateLetters = parkingSessionDto.PlateLetters.Replace(" ", "");

        var userId = _decodeJwt.GetUserIdFromToken(token);
        var user = await _unitOfWork.Employee.FindAsync(r => r.Id == userId);
        if (user == null) throw new InvalidOperationException("User not found.");

        if (!spvalidate.isValidPlate(parkingSessionDto.PlateLetters, parkingSessionDto.PlateNumbers))
            throw new InvalidOperationException("Invalid plate number format.");

        var garage = await _unitOfWork.Garage.FindAsync(e => e.GarageId == user.GarageId);
        if (garage.AvailableParkingSlots < 1)
        {
            return new GeneralResponse
            { Message = "No Space in the garage!" };
        }
        var currentTime = DateTime.UtcNow.AddHours(3);
        var vehicle = await _unitOfWork.Vehicle.FindAsync(v => v.PlateNumbers == parkingSessionDto.PlateNumbers
                                                               && v.PlateLetters == parkingSessionDto.PlateLetters);

        bool isReserved = false;
        Reservation reservation = null;
        if (vehicle != null)
        {
            var reservationStartTimeWindow = currentTime.AddMinutes(-30);
            var reservationEndTimeWindow = currentTime.AddMinutes(30);
            reservation = await _unitOfWork.Reservation.FindAsync(res => res.DriverID == vehicle.DriverId &&
                                                                         res.DateTime >= reservationStartTimeWindow &&
                                                                         res.DateTime <= reservationEndTimeWindow);
            isReserved = reservation != null;
        }
        var isSessionAlready = await _unitOfWork.ParkingSession.FindAsync(e => e.GarageID == user.GarageId &&
        e.PlateLetters == parkingSessionDto.PlateLetters &&
        e.PlateNumbers == parkingSessionDto.PlateNumbers);
        if (isSessionAlready != null)
        {
            return new GeneralResponse
            {
                Success = false,
                Message = "Session already exists"
            };
        }
        ParkingSession parkingSession = new ParkingSession
        {
            StartTime = currentTime,
            VehicleID = vehicle == null ? null : vehicle.VehicleId,
            GarageID = user.GarageId,
            IsRegistered = vehicle != null,
            PlateLetters = parkingSessionDto.PlateLetters,
            PlateNumbers = parkingSessionDto.PlateNumbers,
            IsReserved = isReserved
        };

        if (garage == null) throw new InvalidOperationException("Garage not found.");
        garage.AvailableParkingSlots -= 1;

        // Add a new history entry
        var history = await _unitOfWork.History.AddAsync(new History
        {
            HistoryType = HistroyType.Reservation,
            Timestamp = currentTime
        });
        if (history == null) throw new InvalidOperationException("History error in StartParkingSessionAsync");

        _unitOfWork.ParkingSession.Add(parkingSession);
        await _unitOfWork.SaveChangeAsync();

        // If the reservation exists and is active, add it to ReservationHistory
        if (reservation != null)
        {
            _unitOfWork.ReservationHistory.Add(new ReservationHistory
            {
                ReservationTime = reservation.DateTime,
                DriverId = reservation.DriverID,
                GarageId = reservation.GarageId,
                UsedOrNot = true,
                HistoryId = history.HistoryId
            });
            _unitOfWork.Reservation.Delete(reservation);
            await _unitOfWork.SaveChangeAsync();
        }
        await _unitOfWork.SaveChangeAsync();

        return new GeneralResponse
        {
            Success = true,
            Message = isReserved ? $"Parking session for {parkingSessionDto.PlateLetters} {parkingSessionDto.PlateNumbers} started successfully with Reservation."
                                 : $"Parking session for {parkingSessionDto.PlateLetters} {parkingSessionDto.PlateNumbers} started successfully."
        };
    }

    public async Task<ParkingSessionDetailsDto> EndParkingSessionAsync(EndParkingSessionDto endParkingSessionDto, string token)
    {
        endParkingSessionDto.PlateNumbers = endParkingSessionDto.PlateNumbers.Replace(" ", "");
        endParkingSessionDto.PlateLetters = endParkingSessionDto.PlateLetters.Replace(" ", "");
        var userId = _decodeJwt.GetUserIdFromToken(token);
        var user = await _unitOfWork.Employee.FindAsync(r => r.Id == userId);
        var session = await _unitOfWork.ParkingSession.AsNoTracking()
            .Include(x => x.Vehicle).ThenInclude(h => h.Driver)
            .FirstOrDefaultAsync(g => g.GarageID == user.GarageId
                                      && ((g.PlateLetters == endParkingSessionDto.PlateLetters &&
                                           g.PlateNumbers == endParkingSessionDto.PlateNumbers)));

        if (session == null)
        {
            throw new InvalidOperationException("Parking session not found.");
        }

        var garage = await _unitOfWork.Garage.FindAsync(g => g.GarageId == user.GarageId);
        if (garage == null) throw new InvalidOperationException("Garage not found.");
        garage.AvailableParkingSlots += 1;

        var history = await _unitOfWork.History.AddAsync(new History
        {
            HistoryType = HistroyType.ParkingSession,
            Timestamp = DateTime.UtcNow.AddHours(3),
        });
        if (history == null) throw new InvalidOperationException("History error in EndParkingSession.");
        await _unitOfWork.SaveChangeAsync();

        _unitOfWork.ParkingSessionHistory.Add(new ParkingSessionHistory
        {
            VehicleId = session.VehicleID,
            GarageId = session.GarageID,
            EnterTime = session.StartTime,
            LeaveTime = DateTime.UtcNow.AddHours(3),
            HourlyPrice = garage.HourPrice,
            RequiredPayment = Math.Round((DateTime.UtcNow.AddHours(3) - session.StartTime).TotalMinutes * (garage.HourPrice / 60), 3) + ((session.IsReserved) ? garage.HourPrice : 0),
            ActualPayment = endParkingSessionDto.Payment,
            PlateLetters = session.PlateLetters,
            PlateNumbers = session.PlateNumbers,
            PaymentType = endParkingSessionDto.PaymentType,
            HistoryId = history.HistoryId,
            History = history,
            reserved = session.IsReserved,
            StaffId = userId
        });
        await _unitOfWork.SaveChangeAsync();

        var sessionDetailsDto = new ParkingSessionDetailsDto
        {
            ParkingSessionId = session.ParkingSessionId,
            StartDate = session.StartTime,
            EndDate = DateTime.UtcNow.AddHours(3),
            Bill = Math.Round((DateTime.UtcNow.AddHours(3) - session.StartTime).TotalMinutes * (garage.HourPrice / 60), 3),
            PlateNumbers = session.PlateNumbers,
            PlateLetters = session.PlateLetters,
            DriverName = session.Vehicle?.Driver?.FullName ?? "Unregistered Vehicle",
            SessionDuration = ((int)(DateTime.UtcNow.AddHours(3) - session.StartTime).TotalMinutes / 60).ToString() + ":" + ((int)(DateTime.UtcNow.AddHours(3) - session.StartTime).TotalMinutes % 60).ToString(),
        };

        _unitOfWork.ParkingSession.Delete(session);
        await _unitOfWork.SaveChangeAsync();
        return sessionDetailsDto;
    }

    public async Task<GarageScreenDto> AvailableSpaces(string? token)
    {
        FireAndForgetWarmupRequest();

        var userId = _decodeJwt.GetUserIdFromToken(token);
        var user = await _unitOfWork.Employee.FindAsync(r => r.Id == userId);
        var garage = await _unitOfWork.Garage.FindAsync(g => g.GarageId == user.GarageId);

        return new GarageScreenDto
        {
            AvailableSpaces = garage.AvailableParkingSlots
        };
    }

    private void FireAndForgetWarmupRequest()
    {
        Task.Run(async () =>
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://mohammed321735-rakna-api-gdkgivmppq-ew.a.run.app/Warmup");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to warm up server: {response.StatusCode}");
                }
            }
        }).ConfigureAwait(false);
    }
}