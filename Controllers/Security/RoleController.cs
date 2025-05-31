using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RoleDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RoleDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return role == null ? NotFound() : Ok(role);
        }
    }
}
