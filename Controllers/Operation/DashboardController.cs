using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Controllers.Operation
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("user-activity-details")]
        [ProducesResponseType(typeof(List<UserConceptActivityDto>), 200)]
        public async Task<IActionResult> GetUserActivityDetails([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("Start date cannot be after end date.");

            var activities = await _dashboardService.GetUserConceptActivitiesAsync(startDate, endDate);
            return Ok(activities);
        }

    }
}
