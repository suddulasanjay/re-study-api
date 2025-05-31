using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Utility.Constants;

namespace ReStudyAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDBContext _db;

        public CategoryRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<Category>> GetCategoriesBySubjectIdsAsync(List<int> subjectIds)
        {
            return await _db.Categories.Where(c => subjectIds.Contains(c.SubjectId!.Value) && c.Status == CommonStatus.Enabled).ToListAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _db.Categories.Where(c => c.Status == CommonStatus.Enabled).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.Status == CommonStatus.Enabled);
        }

        public async Task<int> CreateAsync(Category entity)
        {
            entity.CreatedTime = DateTime.UtcNow;
            entity.ModifiedTime = DateTime.UtcNow;
            return await _db.InsertWithInt32IdentityAsync(entity);
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            entity.ModifiedTime = DateTime.UtcNow;
            return await _db.UpdateAsync(entity) > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id, int? modifiedByUserId)
        {
            return await _db.Categories.Where(x => x.Id == id)
                .Set(x => x.Status, CommonStatus.Deleted)
                .Set(x => x.ModifiedByUserId, modifiedByUserId)
                .UpdateAsync() > 0;
        }
    }
}
