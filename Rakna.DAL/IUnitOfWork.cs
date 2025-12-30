using Rakna.DAL.Interfaces;
using Rakna.DAL.Models;
using Rakna.DAL.Models.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Driver> Driver { get; }
        IBaseRepository<Vehicle> Vehicle { get; }
        IBaseRepository<ParkingSession> ParkingSession { get; }
        IBaseRepository<Reservation> Reservation { get; }
        IBaseRepository<Garage> Garage { get; }
        IBaseRepository<Employee> Employee { get; }
        IBaseRepository<Report> Report { get; }
        IBaseRepository<GarageAdmin> GarageAdmin { get; }
        IBaseRepository<CustomerService> CustomerService { get; }
        IBaseRepository<History> History { get; }
        IBaseRepository<ComplaintHistory> ComplaintHistory { get; }
        IBaseRepository<ParkingSessionHistory> ParkingSessionHistory { get; }
        IBaseRepository<ReservationHistory> ReservationHistory { get; }
        IBaseRepository<StaffSalaryHistory> StaffSalaryHistory { get; }
        IBaseRepository<PlateRecognitionHistory> PlateRecognitionHistory { get; }
        IBaseRepository<EmailOTP> EmailOTP { get; }

        Task<int> SaveChangeAsync();

        int SaveChange();
    }
}