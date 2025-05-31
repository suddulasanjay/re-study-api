using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Controllers.Operation
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CategoryDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        public async Task<IActionResult> Create([FromBody] AddCategoryDto dto)
        {
            var id = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] EditCategoryDto dto)
        {
            var success = await _categoryService.UpdateAsync(dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
