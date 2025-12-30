using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Rakna.BAL.DTO.BulkMailDto;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO.Statistics;
using Rakna.BAL.DTO.TechnicalSupportDtos;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.BAL.Service;
using Rakna.DAL.Models;
using System.Security.AccessControl;

namespace Rakna.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "technicalsupport")]
    public class TechnicalSupportController : ControllerBase
    {
        private readonly ITechnicalSupportService _technicalSupport;
        private readonly IEmailSendService _emailSendService;

        public TechnicalSupportController(ITechnicalSupportService technicalSupport, IEmailSendService emailSendService)
        {
            _technicalSupport = technicalSupport;
            _emailSendService = emailSendService;
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get All Garages
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllGarages")]
        public async Task<IActionResult> GetAllGarages()
        {
            var result = _technicalSupport.GetAllGarages();

            return Ok(result.Result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get All Drivers
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllDriversID")]
        public async Task<IActionResult> AllDrivers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _technicalSupport.GetAllDriverID();
            return Ok(result.Result);
        }
        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get All Users (garageadmin,customerservice)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _technicalSupport.GetAllUsers();
            return Ok(result.Result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Add New Garage
        /// </summary>
        /// <param name="garageDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGarage")]
        public async Task<ActionResult> AddGarage(GarageDto garageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Result = _technicalSupport.AddGarage(garageDto);
            return Ok(Result.Result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Update Garage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="garageDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateGarage")]
        public async Task<ActionResult> UpdateGarage(int id, GarageDto garageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _technicalSupport.UpdateGarage(id, garageDto);
            return Ok(Result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Delete Garage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteGarage")]
        public async Task<IActionResult> DeleteGarage(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _technicalSupport.DeleteGarage(id);
            return Ok(Result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Add User
        /// </summary>
        /// <param name="adduserdto">role "<b>garageadmin</b>" or "<b>customerservice</b>"</param>
        /// <returns></returns>

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUserDto adduserdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _technicalSupport.RegisterUser(adduserdto);
            if (!Result.Success)
            {
                return StatusCode(400, Result.Message);
            }
            return Ok(Result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Edit User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("EditUser/{id}")]
        public async Task<ActionResult> EditUser(string id, [FromBody] EditUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _technicalSupport.EditStaffBasedOnRole(id, model);
            if (!result.Success)
            {
                return StatusCode(400, result.Message);
            }

            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _technicalSupport.DeleteUser(id);
            if (!result.Success)
            {
                return StatusCode(400, result.Message);
            }

            return Ok(result);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get All Reports
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllReports")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _technicalSupport.GetAllReports();
            return Ok(reports);
        }

        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get All Reports
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
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get All Drivers
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllBulk")]
        public async Task<IActionResult> GetBulkEmails()
        {
            try
            {
                var token = HttpHelper.GetToken(this.HttpContext);
                var response = await _technicalSupport.GetAllUsersWithRolesAsync(token);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get All Garage Statistics
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetAllGarageStatistics")]
        public async Task<IActionResult> GetAllGarageStatistics([FromQuery] StatisticsRequestDto request)
        {
            try
            {
                var response = await _technicalSupport.GetAllGarageStatistics(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This endpoint is where the TECHNICAL SUPPORT is going to Get Confidence
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetConfidence")]
        public async Task<IActionResult> GetConfidence()
        {
            try
            {
                var response = await _technicalSupport.GetConfidence();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}