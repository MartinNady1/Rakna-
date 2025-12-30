using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rakna.BAL.DTO.AIDto;
using Rakna.BAL.Interface;
using Serilog;

namespace Rakna.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _systemService;
        public SystemController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        [HttpPost("AIConfidenceRate")]
        public async Task<IActionResult> ReceiveDetectionResults([FromBody] DetectionInputDto input)
        {
            if (input == null || input.Results == null || !input.Results.Any())
            {
                return BadRequest("No data provided.");
            }
            try
            {
                var res = await _systemService.AddConfidenceAsync(input.Results, input.Token);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
