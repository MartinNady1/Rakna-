using Rakna.DAL;
using Rakna.DAL.Interfaces;
using Rakna.DAL.Models;
using Rakna.DAL.Models.History;
using Rakna.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.EF
{
    public class UnitOfWork : IUnitOfWork

    {
        private readonly AppDbContext _context;
        public IBaseRepository<Driver> Driver {  get; private set; }
        public IBaseRepository<Vehicle> Vehicle { get; private set; }
        public IBaseRepository<Garage> Garage { get; private set; }
        public IBaseRepository<Reservation> Reservation {  get; private set; }
        public IBaseRepository<Employee> Employee {  get; private set; }
        public IBaseRepository<Report> Report {  get; private set; }
        public IBaseRepository<GarageAdmin> GarageAdmin {  get; private set; }
        public IBaseRepository<CustomerService> CustomerService {  get; private set; }
        public IBaseRepository<ParkingSession> ParkingSession {  get; private set; }
        public IBaseRepository<History> History {  get; private set; }
        public IBaseRepository<ComplaintHistory> ComplaintHistory{  get; private set; }
        public IBaseRepository<ParkingSessionHistory> ParkingSessionHistory {  get; private set; }
        public IBaseRepository<ReservationHistory> ReservationHistory {  get; private set; }
        public IBaseRepository<StaffSalaryHistory> StaffSalaryHistory {  get; private set; }
        public IBaseRepository<PlateRecognitionHistory> PlateRecognitionHistory {  get; private set; }
        public IBaseRepository<EmailOTP> EmailOTP {  get; private set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _context = dbContext;
            Driver = new BaseRepository<Driver>(_context);
            Vehicle = new BaseRepository<Vehicle>(_context);
            ParkingSession = new BaseRepository<ParkingSession>(_context);
            Reservation = new BaseRepository<Reservation>(_context);
            Garage = new BaseRepository<Garage>(_context);
            Employee = new BaseRepository<Employee>(_context);
            Report = new BaseRepository<Report>(_context);
            GarageAdmin = new BaseRepository<GarageAdmin>(_context);
            CustomerService = new BaseRepository<CustomerService>(_context);
            ComplaintHistory = new BaseRepository<ComplaintHistory>(_context);
            ParkingSessionHistory = new BaseRepository<ParkingSessionHistory>(_context);
            ReservationHistory = new BaseRepository<ReservationHistory>(_context);
            StaffSalaryHistory = new BaseRepository<StaffSalaryHistory>(_context);
            History = new BaseRepository<History>(_context);
            EmailOTP = new BaseRepository<EmailOTP>(_context);
            PlateRecognitionHistory = new BaseRepository<PlateRecognitionHistory>(_context);
        }

        public void Dispose()
        {
          _context.Dispose();
        }
     
        public  async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChange()
        {
            return _context.SaveChanges();
        }
    }
}
