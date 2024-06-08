# Rakana - Car Management System

Rakana is a comprehensive car management system developed as a graduation project. The system is designed to facilitate the management of cars, reservations, parking sessions, and reports for various users including drivers, garage staff, and system administrators. The backend API is built using .NET Core, Entity Framework, SQL Database, SignalR, Serilog, Repository Pattern, Unit of Work, and follows a 3-Tier Architecture.

## Table of Contents

- [Project Description](#project-description)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [System Architecture](#system-architecture)
- [Endpoints](#endpoints)
- [Screenshots](#screenshots)
- [Installation](#installation)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## Project Description

Rakana aims to provide a seamless and efficient way for managing car-related operations, including vehicle registration, reservations, parking sessions, and handling complaints. The system is intended to be used by drivers, garage staff, and system administrators to ensure smooth and organized management of car parking and reservations.

## Features

- **User Authentication**: Secure login and registration for drivers.
- **Vehicle Management**: Add, edit, and delete vehicles.
- **Reservation Management**: Make, update, and cancel reservations.
- **Parking Sessions**: Real-time tracking of parking sessions.
- **Report Handling**: Create and manage reports and complaints.
- **User Roles**: Different interfaces and functionalities for drivers, garage staff, and administrators.
- **Real-Time Updates**: Live updates using SignalR for parking sessions and reports.
- **Logging**: Comprehensive logging using Serilog for tracking and debugging.

## Technologies Used

- **.NET Core**: Backend framework.
- **Entity Framework Core**: ORM for database interactions.
- **SQL Server**: Relational database.
- **SignalR**: Real-time web functionality.
- **Serilog**: Logging library.
- **Repository Pattern**: Data access pattern.
- **Unit of Work**: Ensures data integrity.
- **3-Tier Architecture**: Separation of concerns.

## System Architecture

The project follows a 3-Tier Architecture:
- **Presentation Layer**: API Controllers.
- **Business Logic Layer**: Services and SignalR Hubs.
- **Data Access Layer**: Repositories and Entity Framework Context.

## Endpoints

### Auth

- **POST** `/api/Auth/Register`: Register a new driver account.
- **POST** `/api/Auth/Login`: Login for drivers.
- **DELETE** `/api/Auth/DeleteUser`: Delete a user account.
- **GET** `/api/Auth/VerifyEmail`: Verify user email.
- **GET** `/api/Auth/IsEmailVerified`: Check if email is verified.
- **POST** `/api/Auth/RequestPasswordReset`: Request password reset.
- **GET** `/api/Auth/SendResetPassword`: Send reset password link.
- **POST** `/api/Auth/ResetPassword`: Reset password.

### Driver

- **POST** `/api/Driver/AddVehicle`: Add a new vehicle.
- **DELETE** `/api/Driver/DeleteVehicle`: Delete a vehicle by ID.
- **PUT** `/api/Driver/EditVehicle`: Edit vehicle details.
- **GET** `/api/Driver/GetAllVehicle`: Get all vehicles for a driver.
- **POST** `/api/Driver/MakeReservation`: Make a reservation.
- **GET** `/api/Driver/GetAllReservation`: Get all reservations.
- **GET** `/api/Driver/DriverProfileDetails`: Get driver profile details.
- **PUT** `/api/Driver/UpdateDriverDetails`: Update driver details.
- **PUT** `/api/Driver/UpdateDriverPassword`: Update driver password.
- **GET** `/api/Driver/UnsolvedReports`: Get unsolved reports.
- **GET** `/api/Driver/solvedReports`: Get solved reports.
- **GET** `/api/Driver/RealTimeParkingSessions`: Get real-time parking sessions.
- **DELETE** `/api/Driver/CancelReservation`: Cancel a reservation.
- **PUT** `/api/Driver/UpdateReservation`: Update a reservation.

### GarageStaff

- **GET** `/api/GarageStaff/CurrentParkingSessions`: Get current parking sessions.
- **GET** `/api/GarageStaff/AllReservation`: Get all reservations.
- **POST** `/api/GarageStaff/StartParkingSession`: Start a parking session.
- **DELETE** `/api/GarageStaff/EndParkingSession`: End a parking session.
- **GET** `/api/GarageStaff/AvailableSpaces`: Get available parking spaces.

### Report

- **POST** `/api/Report/CreateReport`: Create a new report.
- **GET** `/api/Report/GetAllReports`: Get all reports.
- **GET** `/api/Report/GetAllGarageAdmins`: Get all garage admins.
- **GET** `/api/Report/GetReportsBasedOnRole`: Get reports based on role.
- **PUT** `/api/Report/UpdateReportStatus/{reportId}`: Update report status.
- **POST** `/api/Report/ForwardReport/{reportId}/{reportReceiverId}`: Forward a report.

### TechnicalSupport

- **GET** `/TechnicalSupport/GetAllGarages`: Get all garages.
- **GET** `/TechnicalSupport/AllDriversID`: Get all driver IDs.
- **POST** `/TechnicalSupport/AddGarage`: Add a new garage.
- **PUT** `/TechnicalSupport/UpdateGarage`: Update garage details.
- **DELETE** `/TechnicalSupport/DeleteGarage`: Delete a garage.
- **POST** `/TechnicalSupport/AddUser`: Add a new user.
- **PUT** `/TechnicalSupport/EditUser/{id}`: Edit user details.
- **DELETE** `/TechnicalSupport/DeleteUser/{id}`: Delete a user.
- **GET** `/TechnicalSupport/GetAllReports`: Get all reports.
- **POST** `/TechnicalSupport/SendBulkEmails`: Send bulk emails.
- **GET** `/TechnicalSupport/GetAllBulk`: Get all bulk operations.
- **GET** `/TechnicalSupport/GetAllGarageStatistics`: Get garage statistics.
- **GET** `/TechnicalSupport/GetConfidence`: Get confidence data.

## Screenshots

### Dashboard Page for System Admin
![Dashboard Page](https://your-image-link/dashboard_page.png)

### Dashboard Page for Staff Users
![Dashboard Page for Staff](https://your-image-link/staff_dashboard.png)

### Managing Drivers Page
![Managing Drivers Page](https://your-image-link/managing_drivers.png)

### Complaints Page
![Complaints Page](https://your-image-link/complaints_page.png)

### Employees Page
![Employees Page](https://your-image-link/employees_page.png)

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/rakana-backend.git
    cd rakana-backend
    ```

2. Set up the database:
    - Ensure you have SQL Server installed and running.
    - Update the connection string in `appsettings.json`.

3. Run the migrations to set up the database schema:
    ```bash
    dotnet ef database update
    ```

4. Run the application:
    ```bash
    dotnet run
    ```

## Configuration

The application settings can be configured in `appsettings.json`. Update the connection strings and other settings as needed.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request for any enhancements or bug fixes.

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
