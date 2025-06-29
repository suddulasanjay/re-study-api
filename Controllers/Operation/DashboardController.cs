using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Controllers.Operation
{
    [Authorize]
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
        [ProducesResponseType(typeof(List<UserConceptActivityDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserActivityDetails([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("Start date cannot be after end date.");

            var activities = await _dashboardService.GetUserConceptActivitiesAsync(startDate, endDate);
            return Ok(activities);
        }

        [HttpGet("recent-activities")]
        [ProducesResponseType(typeof(List<RecentActivityDto>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetRecentActivities()
        {
            var activities = await _dashboardService.GetRecentActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("streak-calendar")]
        [ProducesResponseType(typeof(List<StreakDayDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStreakCalendar([FromQuery] int year, [FromQuery] int month)
        {
            var data = await _dashboardService.GetMonthlyStreakAsync(year, month);
            return Ok(data);
        }

        [HttpGet("streak-details")]
        [ProducesResponseType(typeof(StreakDetailsDto), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetStreakDetailsAsync()
        {
            var result = await _dashboardService.GetStreakDetailsAsync();
            return Ok(result);
        }

    }
}
