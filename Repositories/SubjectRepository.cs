using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Utility.Constants;

namespace ReStudyAPI.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDBContext _db;

        public SubjectRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<Subject>> GetSubjectsByUserIdAsync(int userId)
        {
            var userSubjects = from us in _db.UserSubjects
                               join s in _db.Subjects on us.SubjectId equals s.Id
                               where us.UserId == userId && us.Status == CommonStatus.Enabled && s.Status == CommonStatus.Enabled
                               select s;

            var presetSubjects = _db.Subjects.Where(s => s.IsPreset && s.Status == CommonStatus.Enabled);
            return await userSubjects.Union(presetSubjects).ToListAsync();
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await _db.Subjects.Where(s => s.Status == CommonStatus.Enabled).ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _db.Subjects.FirstOrDefaultAsync(s => s.Id == id && s.Status == CommonStatus.Enabled);
        }

        public async Task<int> CreateAsync(Subject entity)
        {
            entity.CreatedTime = DateTime.UtcNow;
            entity.ModifiedTime = DateTime.UtcNow;
            return await _db.InsertWithInt32IdentityAsync(entity);
        }

        public async Task<bool> UpdateAsync(Subject entity)
        {
            entity.ModifiedTime = DateTime.UtcNow;
            return await _db.UpdateAsync(entity) > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id, int? modifiedByUserId)
        {
            return await _db.Subjects.Where(x => x.Id == id)
                .Set(x => x.Status, CommonStatus.Deleted)
                .Set(x => x.ModifiedByUserId, modifiedByUserId)
                .UpdateAsync() > 0;
        }

        public async Task<List<Subject>> GetPresetSubjectsAsync()
        {
            return await _db.Subjects.Where(s => s.IsPreset == true && s.Status == CommonStatus.Enabled).ToListAsync();
        }

        public async Task AssignSubjectToUserAsync(int userId, int subjectId)
        {
            var existingMapping = await _db.UserSubjects.AnyAsync(x => x.UserId == userId && x.SubjectId == subjectId && x.Status == CommonStatus.Enabled);
            if (existingMapping)
            {
                return;
            }
            var userSubject = new UserSubject()
            {
                UserId = userId,
                SubjectId = subjectId,
                CreatedTime = DateTime.UtcNow,
                ModifiedByUserId = userId,
                ModifiedTime = DateTime.UtcNow,
                Status = CommonStatus.Enabled
            };
            await _db.InsertAsync(userSubject);
        }
    }
}
