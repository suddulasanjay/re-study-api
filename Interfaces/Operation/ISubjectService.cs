using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface ISubjectService
    {
        public Task<List<SubjectDto>> GetAllAsync();
        public Task<SubjectDto?> GetByIdAsync(int id);
        public Task<int> CreateAsync(AddSubjectDto subjectDto);
        public Task<bool> UpdateAsync(EditSubjectDto subjectDto);
        public Task<bool> DeleteAsync(int id);
    }
}
