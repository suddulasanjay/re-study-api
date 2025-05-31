using ReStudyAPI.Entities;

namespace ReStudyAPI.Interfaces.Repositories
{
    public interface IConceptRepository : IGenericRepository<Concept>
    {
        public Task<List<Concept>> GetConceptsByCategoryIdsAsync(List<int> categoryIds);
    }
}
