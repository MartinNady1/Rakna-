using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rakna.DAL;

using Rakna.BAL.DTO;
using Rakna.DAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Rakna.BAL.Interface;
using Rakna.BAL.Helper;

namespace Rakna.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "garagestaff")]
    public class GarageStaffController : ControllerBase
    {
        private readonly IGarageStaffService _garageStaffService;

        public GarageStaffController(IGarageStaffService garageStaffService)
        {
            _garageStaffService = garageStaffService;
        }

        /// <summary>
        /// This endpoint is where the GarageStaff is Going to See the current active parking sessions
        /// </summary>

        [HttpGet("CurrentParkingSessions")]
        public async Task<IActionResult> CurrentParkingSessions()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var token = HttpHelper.GetToken(this.HttpContext);

                var sessions = await _garageStaffService.GetCurrentParkingSessionsAsync(token);
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving parking sessions: {ex.Message}" });
            }
        }

        /// <summary>
        /// This endpoint is where the GarageStaff is Going to See the current active Reservations
        /// </summary>
        [HttpGet("AllReservation")]
        public async Task<IActionResult> GetAllReservations()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var token = HttpHelper.GetToken(this.HttpContext);

                var reservations = await _garageStaffService.GetAllReservationsAsync(token);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving reservations: {ex.Message}" });
            }
        }

        /// <summary>
        /// This endpoint is where the GarageStaff is Going to Start the parking session (when the car enters)
        /// </summary>
        [HttpPost("StartParkingSession")]
        public async Task<IActionResult> StartParkingSession([FromBody] StartParkingSessionDto parkingSessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var token = HttpHelper.GetToken(this.HttpContext);

                var sessionDto = await _garageStaffService.StartParkingSessionAsync(parkingSessionDto, token);
                return Ok(sessionDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while starting a parking session: {ex.Message}" });
            }
        }

        /// <summary>
        /// This endpoint is where the GarageStaff is Going to end the parking session and collect the payment (when the car leaves)
        /// </summary>
        /// <param name="endParkingSessionDto">Payment Types "<b>Card</b>" or "<b>Cash</b>" or "<b>Mobile</b>"</param>

        [HttpDelete("EndParkingSession")]
        public async Task<IActionResult> EndParkingSession([FromBody] EndParkingSessionDto endParkingSessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var token = HttpHelper.GetToken(this.HttpContext);

                var sessionDetailsDto = await _garageStaffService.EndParkingSessionAsync(endParkingSessionDto, token);
                return Ok(sessionDetailsDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while ending a parking session: {ex.Message}" });
            }
        }

        /// <summary>
        /// This endpoint is where the screen is monitoring the available parking slots
        /// </summary>
        [HttpGet("AvailableSpaces")]
        public async Task<IActionResult> AvailableSpaces()
        {
            var token = HttpHelper.GetToken(this.HttpContext);

            var result = await _garageStaffService.AvailableSpaces(token);
            return Ok(result);
        }
    }
}