using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Controllers.Security
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
        {
            var user = await _userService.GetAsync();
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        public async Task<IActionResult> Create([FromBody] AddUserDto dto)
        {
            var id = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] EditUserDto dto)
        {
            var success = await _userService.UpdateAsync(dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
