using ReStudyAPI.Entities;

namespace ReStudyAPI.Interfaces.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public Task<List<Subject>> GetSubjectsByUserIdAsync(int userId);
        public Task<List<Subject>> GetPresetSubjectsAsync();
    }
}
