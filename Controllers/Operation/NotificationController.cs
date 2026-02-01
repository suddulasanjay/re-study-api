using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;
using System.Net.Mime;

namespace ReStudyAPI.Controllers.Operation
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(List<NotificationDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notifications = await _notificationService.GetNotificationsAsync();
            return Ok(notifications);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpGet("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var success = await _notificationService.MarkAllAsReadAsync();
            return success ? Ok() : NotFound();
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpGet("mark-as-read/{notificationId:int}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var success = await _notificationService.MarkAsReadAsync(notificationId);
            return success ? Ok() : NotFound();
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(List<string>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpGet("mark-all-as-un-read")]
        public async Task<IActionResult> MarkAllAsUnRead()
        {
            var success = await _notificationService.MarkAllAsUnReadAsync();
            return success ? Ok() : NotFound();
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(List<string>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpGet("mark-as-un-read/{notificationId:int}")]
        public async Task<IActionResult> MarkAsUnRead(int notificationId)
        {
            var success = await _notificationService.MarkAsUnReadAsync(notificationId);
            return success ? Ok() : NotFound();
        }

        [ProducesResponseType(200, Type = typeof(List<string>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpDelete("{notificationId:int}")]
        public async Task<IActionResult> Delete(int notificationId)
        {
            var success = await _notificationService.DeleteAsync(notificationId);
            return success ? Ok() : NotFound();
        }
    }
}
