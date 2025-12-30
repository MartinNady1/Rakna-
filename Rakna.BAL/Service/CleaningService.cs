using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rakna.Common.Enum;
using Rakna.DAL;
using Rakna.DAL.Interfaces;
using Rakna.DAL.Models;
using Rakna.DAL.Models.History;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Service
{
    public class CleaningService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CleaningService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nowUtc = DateTime.UtcNow;
                var nextRunTimeUtc = nowUtc.AddMinutes(30 - nowUtc.Minute % 30).AddSeconds(-nowUtc.Second).AddMilliseconds(-nowUtc.Millisecond);

                var waitTime = nextRunTimeUtc - nowUtc; // Calculate how long to wait in UTC
                if (waitTime.TotalMilliseconds > 0)
                {
                    await Task.Delay(waitTime, stoppingToken); // Wait until the next 30-minute mark
                }

                // Execute the tasks
                await DeleteExpiredOtpsAsync();
                await DeleteExpiredReservationsAsync();
                await UpdateParkingSlotsAsync();
            }
        }
        private async Task UpdateParkingSlotsAsync()
        {
            try
            {
                int c = 0;
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Resolve the Unit of Work from the service scope
                    var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var allGarages = await _unitOfWork.Garage.GetAllAsync();
                    var now = DateTime.UtcNow.AddHours(30);

                    foreach (var garage in allGarages)
                    {
                        if (garage.TotalParkingSlots < garage.AvailableParkingSlots)
                        { garage.AvailableParkingSlots = garage.TotalParkingSlots; }
                        var reservations = await _unitOfWork.Reservation.FindAllAsync(s=>s.GarageId ==  garage.GarageId);
                        int cars = _unitOfWork.ParkingSession.FindAll(s => s.GarageID == garage.GarageId).Count();
                        Log.Information("Cars = " + " " + cars.ToString() + "\n");
                        int numofreservations = 0;
                        foreach (var reservation in reservations)
                        {
                            var reservationTime = reservation.DateTime;

                            if (now >= reservationTime.AddMinutes(-30) && now <= reservationTime.AddMinutes(30))
                            {
                                numofreservations++;
                            }
                            else if (now > reservationTime.AddMinutes(30))
                            {
                                _unitOfWork.Reservation.Delete(reservation);
                                c++;
                            }
                        }
                        garage.AvailableParkingSlots = garage.TotalParkingSlots-(cars + numofreservations);
                        _unitOfWork.Garage.Update(garage);
                        await _unitOfWork.SaveChangeAsync();
                    }
                    Log.Information("Parking slots updated successfully. deleted = " + " " + c.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating parking slots.");
            }
        }
        private async Task DeleteExpiredOtpsAsync()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Resolve the Unit of Work from the service scope
                    var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // Retrieve all OTPs using Unit of Work
                    var allOtps = await _unitOfWork.EmailOTP.GetAllAsync();
                    var expiredOtps = allOtps.Where(otp => otp.ExpiryTime < DateTime.UtcNow).ToList();

                    if (expiredOtps.Count > 0)
                    {
                        _unitOfWork.EmailOTP.DeleteRange(expiredOtps);
                        await _unitOfWork.SaveChangeAsync();

                        Log.Information($"{expiredOtps.Count} expired OTPs deleted.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting expired OTPs.");
            }
        }

        private async Task DeleteExpiredReservationsAsync()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Resolve the Unit of Work from the service scope
                    var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var allReservations = await _unitOfWork.Reservation.GetAllAsync();
                    var expiredReservations = allReservations.Where(e => e.DateTime < DateTime.UtcNow.AddHours(30).AddMinutes(30)).ToList();

                    if (expiredReservations.Count > 0)
                    {
                        foreach (var expiredReservation in expiredReservations)
                        {
                            var history = new History
                            {
                                Timestamp = DateTime.UtcNow,
                                HistoryType = HistroyType.Reservation
                            };

                            await _unitOfWork.History.AddAsync(history);
                            await _unitOfWork.SaveChangeAsync();

                            var reservationHistory = new ReservationHistory
                            {
                                DriverId = expiredReservation.DriverID,
                                GarageId = expiredReservation.GarageId,
                                ReservationTime = expiredReservation.DateTime,
                                UsedOrNot = false,
                                HistoryId = history.HistoryId,
                                History = history,
                                ReservationId = expiredReservation.ReservationId,
                            };
                            await _unitOfWork.ReservationHistory.AddAsync(reservationHistory);
                        }
                        _unitOfWork.Reservation.DeleteRange(expiredReservations);
                        await _unitOfWork.SaveChangeAsync();

                        Log.Information($"{expiredReservations.Count} expired reservations processed and history records created.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting expired Reservations.");
            }
        }
    }
}