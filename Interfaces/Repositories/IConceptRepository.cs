using ReStudyAPI.Entities;
using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Repositories
{
    public interface IConceptRepository : IGenericRepository<Concept>
    {
        public Task<List<Concept>> GetConceptsByCategoryIdsAsync(List<int> categoryIds);
        public Task<bool> TrackUserConceptActivityAsync(UserConceptActivity activity);
        public Task<StudySessionDto> GetStudySessionDetailsAsync(int conceptId, int userId, DateTime date);
    }
}
