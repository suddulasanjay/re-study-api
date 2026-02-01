using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface IConceptService
    {
        public Task<List<ConceptDto>> GetAllAsync();
        public Task<ConceptDto?> GetByIdAsync(int id);
        public Task<int> CreateAsync(AddConceptDto dto);
        public Task<bool> UpdateAsync(EditConceptDto dto);
        public Task<bool> DeleteAsync(int id);
        public Task<bool> RecordStudySessionAsync(AddStudySessionDto dto);
        public Task<StudySessionDto> GetStudySessionDetailsAsync(int conceptId);
    }
}
