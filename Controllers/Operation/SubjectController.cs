using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;
using System.Net.Mime;

namespace ReStudyAPI.Controllers.Operation
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(List<SubjectDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(SubjectDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null) return NotFound();
            return Ok(subject);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] AddSubjectDto subjectDto)
        {
            var id = await _subjectService.CreateAsync(subjectDto);
            return CreatedAtAction(nameof(GetSubject), new { id }, subjectDto);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(204, Type = typeof(bool))]
        [ProducesResponseType(404, Type = typeof(string))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject([FromBody] EditSubjectDto subjectDto)
        {
            var updated = await _subjectService.UpdateAsync(subjectDto);
            return updated ? NoContent() : NotFound();
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(204, Type = typeof(bool))]
        [ProducesResponseType(404, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var deleted = await _subjectService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
