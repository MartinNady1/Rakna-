using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Rakna.BAL.DTO.DriverDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.ReportDto;
using Rakna.BAL.DTO.UpdateDriverDto;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.BAL.Service;
using Rakna.DAL.Models;
using Serilog;
using System.Text;

namespace Rakna.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "driver")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _DriverService;
        private readonly IDecodeJwt _decodeJwt;

        public DriverController(IDriverService DriverService, IDecodeJwt decodeJwt)
        {
            _DriverService = DriverService;
            _decodeJwt = decodeJwt;
        }

        /// <summary>
        /// This endpoint is where the driver is adding his cars, the id is taken from the token of the signin
        /// </summary>
        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto addVehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Request = _DriverService.AddVehicle(addVehicleDto, token);
            if (!Request.Result.Success)
            {
                return BadRequest(Request.Result);
            }
            return Ok(Request.Result);
        }
        /// <summary>
        /// used for deleting a vehicle by id
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteVehicle")]
        public async Task<IActionResult> DeleteVehicle(int VehicleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var Request = _DriverService.DeleteVehicle(VehicleId);
            if (!Request.Result.Success)
            {
                return BadRequest(Request.Result);
            }
            return Ok(Request.Result);
        }
        /// <summary>
        /// used for editting a vehicle 
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <returns></returns>
        [HttpPut("EditVehicle")]
        public async Task<IActionResult> EditVehicle(int VehicleId, VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Request = _DriverService.EditVehicle(VehicleId, vehicleDto);
            if (!Request.Result.Success)
            {
                return BadRequest(Request.Result);
            }
            return Ok(Request.Result);
        }
        /// <summary>
        /// return all Vehicle for driver
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllVehicle")]
        public async Task<IActionResult> GetAllVehicle()
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var Result = _DriverService.GetAllVehicle(token);
            return Ok(Result.Result);

        }
        /// <summary>
        /// This endpoint is where the Driver is Making a reservation using the token from his login
        /// </summary>
        [HttpPost("MakeReservation")]
        public async Task<IActionResult> SessionResrvation(DriverReservationDto reservationDto)
        {

            var token = HttpHelper.GetToken(this.HttpContext);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _DriverService.Reservation(reservationDto, token);
            if (!Result.Success)
            {
                return BadRequest(Result);
            }
            return Ok(Result);
        }

        /// <summary>
        /// This endpoint is where the Driver is going to see his reservations
        /// </summary>
        [HttpGet("GetAllReservation")]
        public async Task<IActionResult> GetAllReservation()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var reservationDetails = await _DriverService.ReservationDetails(token);
            if (reservationDetails == null)
            {
                return NoContent();
            }
            return Ok(reservationDetails);
        }

        /// <summary>
        /// This endpoint is where the Driver is Going to see his details
        /// </summary>
        [HttpGet("DriverProfileDetails")]
        public async Task<IActionResult> DriverProfileDetails()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var result = _DriverService.DriverProfileDetails(token);

            if (result == null)
            {
                return NoContent();
            }
            return Ok(result.Result);
        }

        /// <summary>
        /// This endpoint is where the Driver is Going Update his details
        /// </summary>

        [HttpPut("UpdateDriverDetails")]
        public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverDetailsDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var result = await _DriverService.UpdateDriverDetails(token, updateDto);

            if (!result.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the Driver is Going to Change his password
        /// </summary>
        [HttpPut("UpdateDriverPassword")]
        public async Task<IActionResult> UpdateDriverPassword([FromBody] UpdateDriverPasswordDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var result = await _DriverService.UpdateDriverPassword(token, updateDto);

            if (!result.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }

        /// <summary>
        /// This endpoint is returning unsolved reports
        /// </summary>
        [HttpGet("UnsolvedReports")]
        public async Task<IActionResult> UnsolvedDriverReport()
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var report = _DriverService.UnsolvedDriversReport(token);
            if (report.IsNullOrEmpty())
            {
                return BadRequest("No reports found for this user");
            }
            return Ok(report);
        }

        /// <summary>
        /// This endpoint is returning all solved reports
        /// </summary>
        [HttpGet("solvedReports")]
        public async Task<IActionResult> solvedDriverReport()
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var report = _DriverService.solvedDriversReport(token);
            if (report.IsNullOrEmpty())
            {
                return BadRequest("No reports found for this user");
            }
            return Ok(report);
        }
        [HttpGet("RealTimeParkingSessions")]
        public async Task<IActionResult> RealTimeParkingSessions()
        {
            try
            {
                var token = HttpHelper.GetToken(this.HttpContext);
                var report = await _DriverService.RealTimeParkingSessionsDetails(token);  // Ensure this is awaited if it's an async method
                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception details here to understand what went wrong
                return BadRequest($"Error in realtime parking session: {ex.Message}");
            }
        }
        /// <summary>
        /// This endpoint used for canceling a reservation
        /// </summary>
        [HttpDelete("CancelReservation")]
        public async Task<IActionResult> CancelReservation(int ReservationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Request = _DriverService.CancelReservation(ReservationId);
            if (!Request.Result.Success)
            {
                return BadRequest(Request.Result);
            }
            return Ok(Request.Result);
        }
        /// <summary>
        /// This endpoint used to update a reservation (garage or time or both)
        /// </summary>
        [HttpPut("UpdateReservation")]

        public async Task<IActionResult> EditReservation(int ResevationId, DriverReservationDto reservationDto)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var Request = _DriverService.EditReservation(token, ResevationId, reservationDto);
            if (!Request.Result.Success)
            {
                return BadRequest(Request.Result);
            }
            return Ok(Request.Result);
        }
    }
}
