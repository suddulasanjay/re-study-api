using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Utility.Constants;

namespace ReStudyAPI.Services.Operation
{
    public class SubjectService : ISubjectService
    {
        private readonly AppDBContext _db;
        public SubjectService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(AddSubjectDto subjectDto)
        {
            var subject = new Subject
            {
                Name = subjectDto.Name,
                Description = subjectDto.Description,
                IsPreset = subjectDto.IsPreset,
                Status = CommonStatus.Enabled,
                CreatedTime = DateTime.UtcNow,
                ModifiedTime = DateTime.UtcNow,
                ModifiedByUserId = 1
            };
            return await _db.InsertWithInt32IdentityAsync(subject);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _db.Subjects.Where(x => x.Id == id).Set(x => x.Status, CommonStatus.Deleted).UpdateAsync();
            return true;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllAsync()
        {
            var subjects = await _db.Subjects.Where(x => x.Status == CommonStatus.Enabled).Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                IsPreset = s.IsPreset
            }).ToListAsync();
            return subjects;
        }

        public async Task<SubjectDto?> GetByIdAsync(int id)
        {
            return await _db.Subjects.Where(x => x.Status == CommonStatus.Enabled).Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                IsPreset = s.IsPreset
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(EditSubjectDto subjectDto)
        {
            await _db.Subjects.Where(x => x.Id == subjectDto.Id)
                .Set(x => x.Description, subjectDto.Description)
                .Set(s => s.ModifiedTime, DateTime.UtcNow)
                .UpdateAsync();
            return true;
        }
    }
}
