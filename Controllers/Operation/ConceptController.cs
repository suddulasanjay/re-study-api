using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Controllers.Operation
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConceptController : ControllerBase
    {
        private readonly IConceptService _conceptService;

        public ConceptController(IConceptService conceptService)
        {
            _conceptService = conceptService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ConceptDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var concepts = await _conceptService.GetAllAsync();
            return Ok(concepts);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ConceptDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var concept = await _conceptService.GetByIdAsync(id);
            return concept == null ? NotFound() : Ok(concept);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        public async Task<IActionResult> Create([FromBody] AddConceptDto dto)
        {
            var id = await _conceptService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] EditConceptDto dto)
        {
            var success = await _conceptService.UpdateAsync(dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _conceptService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("record-study-session")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> RecordStudySession([FromBody] AddStudySessionDto dto)
        {
            if (dto == null || dto.ConceptId <= 0 || dto.Duration <= 0)
            {
                return BadRequest("Invalid study session data");
            }
            var result = await _conceptService.RecordStudySessionAsync(dto);
            return Ok(result);

        }

        [HttpGet("study-session/{conceptId:int}")]
        [ProducesResponseType(typeof(StudySessionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StudySessionDto>> GetStudySession(int conceptId)
        {
            var result = await _conceptService.GetStudySessionDetailsAsync(conceptId);
            return Ok(result);

        }


    }
}
