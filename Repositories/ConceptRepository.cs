using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Utility.Constants;

namespace ReStudyAPI.Repositories
{
    public class ConceptRepository : IConceptRepository
    {
        private readonly AppDBContext _db;

        public ConceptRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<Concept>> GetConceptsByCategoryIdsAsync(List<int> categoryIds)
        {
            return await _db.Concepts.Where(c => categoryIds.Contains(c.CategoryId) && c.Status == CommonStatus.Enabled).ToListAsync();
        }

        public async Task<List<Concept>> GetAllAsync()
        {
            return await _db.Concepts.Where(c => c.Status == CommonStatus.Enabled).ToListAsync();
        }

        public async Task<Concept?> GetByIdAsync(int id)
        {
            return await _db.Concepts.FirstOrDefaultAsync(c => c.Id == id && c.Status == CommonStatus.Enabled);
        }

        public async Task<int> CreateAsync(Concept entity)
        {
            entity.CreatedTime = DateTime.UtcNow;
            entity.ModifiedTime = DateTime.UtcNow;
            return await _db.InsertWithInt32IdentityAsync(entity);
        }

        public async Task<bool> UpdateAsync(Concept entity)
        {
            entity.ModifiedTime = DateTime.UtcNow;
            return await _db.UpdateAsync(entity) > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id, int? modifiedByUserId)
        {
            return await _db.Concepts.Where(x => x.Id == id)
                .Set(x => x.Status, CommonStatus.Deleted)
                .Set(x => x.ModifiedByUserId, modifiedByUserId)
                .UpdateAsync() > 0;
        }
    }
}
