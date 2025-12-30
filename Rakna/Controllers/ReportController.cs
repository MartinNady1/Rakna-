using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rakna.BAL.DTO.ReportDto;
using Rakna.BAL.Interface;
using Rakna.BAL.Helper;
using System.Threading.Tasks;

namespace Rakna.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary> This endpoint is where the GarageStaff,GarageAdmin,Driver are Going to create complaints </summary>
        /// <param name="addReportDto">Complaint Type "<b>Other</b>" or "<b>SystemError</b>" or "<b>BillingError</b>" or "<b>ServiceDelay</b>" or "<b>EquipmentIssue</b>" or "<b>PolicyViolation</b>" or "<b>CustomerFeedback</b>"</param>

        [HttpPost("CreateReport")]
        [Authorize(Policy = "ReportWriter")]
        public async Task<IActionResult> CreateReport([FromBody] AddReportDto addReportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var response = await _reportService.CreateReportAsync(addReportDto, token);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// This endpoint is where the Customerservice is Going to read all complaints
        /// </summary>
        [HttpGet("GetAllReports")]
        [Authorize(Roles = "customerservice")]
        public IActionResult GetAllReports(int turn)
        {
            var reports = _reportService.GetAllReports(turn);
            return Ok(reports);
        }

        /// <summary>
        /// This endpoint is where the Customerservice is Going to get all garageadmins
        /// </summary>
        [HttpGet("GetAllGarageAdmins")]
        [Authorize(Policy = "GetAllGrageAdmins")]
        public IActionResult GetAllGarageAdmins()
        {
            var reports = _reportService.GetAllGarageadmin();
            return Ok(reports);
        }

        /// <summary>
        /// This endpoint is where the Technical Support or the GarageAdmin are Going to see complaints that are forwarded to them
        /// </summary>
        [HttpGet("GetReportsBasedOnRole")]
        public IActionResult GetReportsBasedOnRole()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var reports = _reportService.GetReportsBasedOnRoleAsync(token);
            return Ok(reports);
        }

        /// <summary>
        /// This endpoint is where the TechnicalSupport,GarageAdmin,CustomerService are Going to update complaints to fixed and add them to the history
        /// </summary>
        [HttpPut("UpdateReportStatus/{reportId}")]
        public async Task<IActionResult> UpdateReportStatus(int reportId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var response = await _reportService.UpdateReportStatusAsync(reportId, token);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// This endpoint is where the customer support is Going to Forward complaints
        /// </summary>

        [HttpPost("ForwardReport/{reportId}/{reportReceiverId}")]
        public async Task<IActionResult> ForwardReport(int reportId, string reportReceiverId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpHelper.GetToken(this.HttpContext);

            var response = await _reportService.ForwardReport(reportId, token, reportReceiverId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}