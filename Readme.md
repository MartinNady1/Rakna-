# Rakna - Parking Management System

Rakana is a comprehensive parking management system developed as a graduation project. The system is designed to facilitate the management of parking spaces, reservations, and user interactions for various users including drivers, garage staff, and system administrators. The backend API is built using .NET Core, Entity Framework, SQL Database, SignalR, Serilog, Repository Pattern, Unit of Work, and follows a 3-Tier Architecture.

## Table of Contents

- [Project Description](#project-description)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [System Architecture](#system-architecture)
- [Endpoints](#endpoints)
  - [Auth Endpoints](#auth-endpoints)
  - [Driver Endpoints](#driver-endpoints)
  - [Garage Staff Endpoints](#garage-staff-endpoints)
  - [Report Endpoints](#report-endpoints)
  - [Technical Support Endpoints](#technical-support-endpoints)
- [Screenshots](#screenshots)
- [Installation](#installation)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## Project Description

Rakana aims to provide a seamless and efficient way to manage parking-related operations, including space allocation, reservations, real-time tracking of parking sessions, and handling complaints. The system is intended to be used by drivers, garage staff, and system administrators to ensure smooth and organized management of parking facilities.

## Features

- **User Authentication**: Secure login and registration for drivers.
- **Parking Space Management**: Add, edit, and delete parking spaces.
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

### Auth Endpoints

- **POST** `/api/Auth/Register`
  - **Description**: Registers a new driver account. The driver provides necessary information such as name, email, and password. The email must be unique and is used for verification purposes.
  - **Request Parameters**: `name`, `email`, `password`
  - **Response**: Success message and user details.

- **POST** `/api/Auth/Login`
  - **Description**: Authenticates a driver using their email and password. If the credentials are correct, a JWT token is issued for subsequent requests.
  - **Request Parameters**: `email`, `password`
  - **Response**: JWT token and user details.

- **DELETE** `/api/Auth/DeleteUser`
  - **Description**: Deletes a user account permanently from the system. Only authenticated users can delete their own account.
  - **Request Parameters**: `userId`
  - **Response**: Success message.

- **GET** `/api/Auth/VerifyEmail`
  - **Description**: Verifies the driver's email using a token sent to their email address during registration.
  - **Request Parameters**: `email`, `verificationToken`
  - **Response**: Success message.

- **GET** `/api/Auth/IsEmailVerified`
  - **Description**: Checks if the logged-in user's email has been verified.
  - **Request Parameters**: None (uses JWT token for authentication)
  - **Response**: Verification status.

- **POST** `/api/Auth/RequestPasswordReset`
  - **Description**: Initiates a password reset process by sending a reset link to the user's email.
  - **Request Parameters**: `email`
  - **Response**: Success message.

- **GET** `/api/Auth/SendResetPassword`
  - **Description**: Sends a password reset link to the user's email address.
  - **Request Parameters**: `email`
  - **Response**: Success message.

- **POST** `/api/Auth/ResetPassword`
  - **Description**: Resets the user's password using the token received via email.
  - **Request Parameters**: `email`, `resetToken`, `newPassword`
  - **Response**: Success message.

### Driver Endpoints

- **POST** `/api/Driver/AddVehicle`
  - **Description**: Allows a driver to add a new vehicle to their profile. The driver must provide vehicle details such as make, model, and registration number.
  - **Request Parameters**: `make`, `model`, `registrationNumber`
  - **Response**: Success message and vehicle details.

- **DELETE** `/api/Driver/DeleteVehicle`
  - **Description**: Deletes a vehicle from the driver's profile using the vehicle ID.
  - **Request Parameters**: `vehicleId`
  - **Response**: Success message.

- **PUT** `/api/Driver/EditVehicle`
  - **Description**: Allows the driver to edit the details of an existing vehicle.
  - **Request Parameters**: `vehicleId`, `make`, `model`, `registrationNumber`
  - **Response**: Success message and updated vehicle details.

- **GET** `/api/Driver/GetAllVehicle`
  - **Description**: Retrieves all vehicles associated with the logged-in driver.
  - **Request Parameters**: None
  - **Response**: List of vehicles.

- **POST** `/api/Driver/MakeReservation`
  - **Description**: Allows a driver to make a parking reservation. The driver must specify the parking lot, date, and time for the reservation.
  - **Request Parameters**: `parkingLotId`, `date`, `startTime`, `endTime`
  - **Response**: Success message and reservation details.

- **GET** `/api/Driver/GetAllReservation`
  - **Description**: Retrieves all reservations made by the logged-in driver.
  - **Request Parameters**: None
  - **Response**: List of reservations.

- **GET** `/api/Driver/DriverProfileDetails`
  - **Description**: Retrieves the profile details of the logged-in driver.
  - **Request Parameters**: None
  - **Response**: Driver profile details.

- **PUT** `/api/Driver/UpdateDriverDetails`
  - **Description**: Allows the driver to update their profile details.
  - **Request Parameters**: `name`, `email`, `phoneNumber`
  - **Response**: Success message and updated profile details.

- **PUT** `/api/Driver/UpdateDriverPassword`
  - **Description**: Allows the driver to change their password.
  - **Request Parameters**: `currentPassword`, `newPassword`
  - **Response**: Success message.

- **GET** `/api/Driver/UnsolvedReports`
  - **Description**: Retrieves all unsolved reports filed by the driver.
  - **Request Parameters**: None
  - **Response**: List of unsolved reports.

- **GET** `/api/Driver/solvedReports`
  - **Description**: Retrieves all solved reports filed by the driver.
  - **Request Parameters**: None
  - **Response**: List of solved reports.

- **GET** `/api/Driver/RealTimeParkingSessions`
  - **Description**: Provides real-time updates on the driver's active parking sessions.
  - **Request Parameters**: None
  - **Response**: List of active parking sessions.

- **DELETE** `/api/Driver/CancelReservation`
  - **Description**: Allows the driver to cancel an existing reservation.
  - **Request Parameters**: `reservationId`
  - **Response**: Success message.

- **PUT** `/api/Driver/UpdateReservation`
  - **Description**: Allows the driver to update an existing reservation, such as changing the time or parking lot.
  - **Request Parameters**: `reservationId`, `parkingLotId`, `date`, `startTime`, `endTime`
  - **Response**: Success message and updated reservation details.

### Garage Staff Endpoints

- **GET** `/api/GarageStaff/CurrentParkingSessions`
  - **Description**: Retrieves a list of current parking sessions in the garage.
  - **Request Parameters**: None
  - **Response**: List of current parking sessions.

- **GET** `/api/GarageStaff/AllReservation`
  - **Description**: Retrieves a list of all reservations for the garage.
  - **Request Parameters**: None
  - **Response**: List of reservations.

- **POST** `/api/GarageStaff/StartParkingSession`
  - **Description**: Starts a new parking session when a car enters the garage.
  - **Request Parameters**: `vehicleId`, `parkingLotId`
  - **Response**: Success message and parking session details.

- **DELETE** `/api/GarageStaff/EndParkingSession`
  - **Description**: Ends an active parking session when a car leaves the garage and processes the payment.
  - **Request Parameters**: `sessionId`
  - **Response**: Success message and payment details.

- **GET** `/api/GarageStaff/AvailableSpaces`
  - **Description**: Retrieves the number of available parking spaces in the garage.
  - **Request Parameters**: None
  - **Response**: Number of available spaces.

### Report Endpoints

- **POST** `/api/Report/CreateReport`
  - **Description**: Allows users to create a new report or complaint regarding any issues they face.
  - **Request Parameters**: `title`, `description`, `category`
  - **Response**: Success message and report details.

- **GET** `/api/Report/GetAllReports`
  - **Description**: Retrieves all reports created in the system.
  - **Request Parameters**: None
  - **Response**: List of reports.

- **GET** `/api/Report/GetAllGarageAdmins`
  - **Description**: Retrieves a list of all garage administrators.
  - **Request Parameters**: None
  - **Response**: List of garage administrators.

- **GET** `/api/Report/GetReportsBasedOnRole`
  - **Description**: Retrieves reports based on the user's role (e.g., technical support, garage admin).
  - **Request Parameters**: `role`
  - **Response**: List of reports.

- **PUT** `/api/Report/UpdateReportStatus/{reportId}`
  - **Description**: Updates the status of a specific report, marking it as solved or in-progress.
  - **Request Parameters**: `status`
  - **Response**: Success message and updated report details.

- **POST** `/api/Report/ForwardReport/{reportId}/{reportReceiverId}`
  - **Description**: Forwards a report to another user or department for further action.
  - **Request Parameters**: `reportReceiverId`
  - **Response**: Success message.

### Technical Support Endpoints

- **GET** `/TechnicalSupport/GetAllGarages`
  - **Description**: Retrieves a list of all garages in the system.
  - **Request Parameters**: None
  - **Response**: List of garages.

- **GET** `/TechnicalSupport/AllDriversID`
  - **Description**: Retrieves a list of all driver IDs in the system.
  - **Request Parameters**: None
  - **Response**: List of driver IDs.

- **POST** `/TechnicalSupport/AddGarage`
  - **Description**: Adds a new garage to the system.
  - **Request Parameters**: `garageName`, `location`, `capacity`
  - **Response**: Success message and garage details.

- **PUT** `/TechnicalSupport/UpdateGarage`
  - **Description**: Updates the details of an existing garage.
  - **Request Parameters**: `garageId`, `garageName`, `location`, `capacity`
  - **Response**: Success message and updated garage details.

- **DELETE** `/TechnicalSupport/DeleteGarage`
  - **Description**: Deletes a garage from the system.
  - **Request Parameters**: `garageId`
  - **Response**: Success message.

- **POST** `/TechnicalSupport/AddUser`
  - **Description**: Adds a new user (driver, admin, etc.) to the system.
  - **Request Parameters**: `name`, `email`, `password`, `role`
  - **Response**: Success message and user details.

- **PUT** `/TechnicalSupport/EditUser/{id}`
  - **Description**: Updates the details of an existing user.
  - **Request Parameters**: `name`, `email`, `role`
  - **Response**: Success message and updated user details.

- **DELETE** `/TechnicalSupport/DeleteUser/{id}`
  - **Description**: Deletes a user from the system.
  - **Request Parameters**: `userId`
  - **Response**: Success message.

- **GET** `/TechnicalSupport/GetAllReports`
  - **Description**: Retrieves all reports and complaints in the system.
  - **Request Parameters**: None
  - **Response**: List of reports.

- **POST** `/TechnicalSupport/SendBulkEmails`
  - **Description**: Sends bulk emails to users for notifications or updates.
  - **Request Parameters**: `emailList`, `subject`, `message`
  - **Response**: Success message.

- **GET** `/TechnicalSupport/GetAllBulk`
  - **Description**: Retrieves all bulk operations performed in the system.
  - **Request Parameters**: None
  - **Response**: List of bulk operations.

- **GET** `/TechnicalSupport/GetAllGarageStatistics`
  - **Description**: Retrieves statistics for all garages, including occupancy rates and revenue.
  - **Request Parameters**: None
  - **Response**: Garage statistics.

- **GET** `/TechnicalSupport/GetConfidence`
  - **Description**: Retrieves confidence data based on system performance and user feedback.
  - **Request Parameters**: None
  - **Response**: Confidence data.

## Screenshots

### Dashboard Page for System Admin
![Dashboard Page](1.png)

### Dashboard Page for Staff Users
![Dashboard Page for Staff](2.png)

### Managing Drivers Page
![Managing Drivers Page](3.png)

### Complaints Page
![Complaints Page](4.png)

### Employees Page
![Employees Page](5.png)

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request for any enhancements or bug fixes.

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a pull request.
