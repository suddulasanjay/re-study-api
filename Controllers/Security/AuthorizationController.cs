using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("token")]
        [ProducesResponseType(typeof(SSOTokenResponseDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(object), 401)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Exchange(SSOTokenRequestDto request)
        {
            try
            {
                var tokenResponse = await _authorizationService.GetToken(request);
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
        }
    }
}
