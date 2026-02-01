using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Controllers.Operation
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("todays-agenda")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AgendaDto>>> GetAgenda()
        {
            var agenda = await _homeService.GetAgendaAsync();
            return Ok(agenda);
        }
    }
}
