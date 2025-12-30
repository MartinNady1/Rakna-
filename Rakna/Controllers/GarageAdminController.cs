using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rakna.BAL.DTO.BulkMailDto;
using Rakna.BAL.DTO.GarageStaffDto;
using Rakna.BAL.DTO.Statistics;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.BAL.Service;
using Rakna.DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rakna.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "garageadmin")]
    public class GarageAdminController : ControllerBase
    {
        private readonly IGarageAdminService _garageAdminService;
        private readonly IEmailSendService _emailSendService;

        public GarageAdminController(IGarageAdminService garageAdminService, IEmailSendService emailSendService)
        {
            _garageAdminService = garageAdminService;
            _emailSendService = emailSendService;
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Add Staff
        /// </summary>
        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff(AddStaffDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.AddStaff(model, token);
            if (!result.Success)
            {
                return StatusCode(400, result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Delete Staff
        /// </summary>
        [HttpDelete("DeleteStaff/{id}")]
        public async Task<IActionResult> DeleteStaff(string id)
        {
            var result = await _garageAdminService.DeleteStaff(id);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Change his own garage hour price
        /// </summary>

        [HttpPut("EditHourPrice")]
        public async Task<IActionResult> EditHourPrice([FromBody] int newHourPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.EditHourPrice(newHourPrice, token);
            if (!result.Success)
            {
                return StatusCode(400, result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Edit Staff Information
        /// </summary>

        [HttpPut("EditStaff/{id}")]
        public async Task<IActionResult> EditStaff(string id, [FromBody] AddStaffDTO model)
        {
            var result = await _garageAdminService.EditStaff(id, model);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to See a list with all the Staff
        /// </summary>
        [HttpGet("AllStaff")]
        public IActionResult Allstaff()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = _garageAdminService.getAllStaff(token);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to See All the active parking sessions
        /// </summary>
        [HttpGet("ActiveSessions")]
        public async Task<IActionResult> ActiveSessions()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var result = _garageAdminService.gettAllSession(token);
            if (result.Result == null)
            {
                return NoContent();
            }
            return Ok(result.Result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to See All the Paid Salaries
        /// </summary>
        [HttpGet("GetAllPaidSalaries")]
        public ActionResult GetAllPaidSalaries()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = _garageAdminService.GetAllStaffpaidSalary(token);
            if (result != null)
                return Ok(result.Result);
            else
                return NotFound("No paid salaries found.");
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to See All the Unpaid Salaries
        /// </summary>
        [HttpGet("GetAllUnpaidSalaries")]
        public ActionResult GetAllUnpaidSalaries()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = _garageAdminService.GetAllStaffUnpaidSalary(token);
            if (result != null)
                return Ok(result.Result);
            else
                return NotFound("No unpaid salaries found.");
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Get All Staff Salaries
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllStaffSalaries")]
        public ActionResult GetAllStaffSalaries()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = _garageAdminService.GetAllStaffSalaries(token);
            if (result != null)
                return Ok(result.Result);
            else
                return NotFound("Error while getting all staff salaries");
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Pay his staff salary based on his id
        /// </summary>
        [HttpPost("PaySalary/{id}")]
        public async Task<IActionResult> PaySalary(string id)
        {
            var response = await _garageAdminService.PaySalary(id);
            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Send Bulk Emails to selected staff
        /// </summary>
        /// <param name="sendBulkEmailDto"></param>
        /// <returns></returns>

        [HttpPost("SendBulkEmails")]
        public async Task<IActionResult> SendBulkEmails([FromBody] SendBulkEmailDto sendBulkEmailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);
            var response = await _emailSendService.SendBulkEmails(sendBulkEmailDto.Emails, sendBulkEmailDto.Message, sendBulkEmailDto.Title, token);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Get All Staff Emails
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllStaffBulk")]
        public async Task<IActionResult> GetBulkEmails()
        {
            try
            {
                var token = HttpHelper.GetToken(this.HttpContext);
                var response = await _garageAdminService.getAllStaffbulk(token);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Calculate Average Parking Duration Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet("CalculateAverageParkingDuration")]
        public async Task<IActionResult> CalculateAverageParkingDuration([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateAverageParkingDurationAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Calculate Total Revenue Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet("CalculateTotalRevenue")]
        public async Task<IActionResult> CalculateTotalRevenue([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateTotalRevenueAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Calculate Total Reservations Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet("CalculateTotalReservations")]
        public async Task<IActionResult> CalculateTotalReservations([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateTotalReservationsAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Calculate Total Salary Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet("CalculateTotalSalaryPaid")]
        public async Task<IActionResult> CalculateTotalSalaryPaid([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateTotalSalaryPaidAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Calculate Staff Activity Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet("CalculateStaffActivityRating")]
        public async Task<IActionResult> CalculateStaffActivityRating([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateStaffActivityRatingAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Calculate Complaints Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("CalculateComplaintsStatistics")]
        public async Task<IActionResult> CalculateComplaintsStatistics([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateComplaintsStatisticsAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Get Peak Parking Hours Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet("GetPeakParkingHours")]
        public async Task<IActionResult> GetPeakParkingHours([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.GetPeakParkingHoursAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Get Reserved Vs Non Reserved Parking Usage Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetReservedVsNonReservedParkingUsage")]
        public async Task<IActionResult> GetReservedVsNonReservedParkingUsage([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.GetReservedVsNonReservedParkingUsageAsync(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Get All Statistics By refrencing all the above endpoints (Long time)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetAllStatsSlow")]
        public async Task<IActionResult> GetAllStatsSlow([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateGarageStatisticsAsyncSlow(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Get All Statistics Fast by removing the duplicates and do once
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetAllStatsFast")]
        public async Task<IActionResult> GetAllStatsFast([FromQuery] StatisticsRequestDto request)
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.CalculateGarageStatisticsAsyncFast(request, token);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the GarageAdmin is Going to Get Garage Data and hours
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGarageData")]
        public async Task<IActionResult> GetGarageData()
        {
            var token = HttpHelper.GetToken(this.HttpContext);
            var result = await _garageAdminService.GetGarageData(token);
            return Ok(result);
        }
    }
}