using ReStudyAPI.Entities;

namespace ReStudyAPI.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<List<Category>> GetCategoriesBySubjectIdsAsync(List<int> subjectIds);
    }
}
