using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Models.Operation;
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

        public async Task<bool> TrackUserConceptActivityAsync(UserConceptActivity activity)
        {
            var existingActivity = await _db.UserConceptActivities.FirstOrDefaultAsync(x => x.ConceptId == activity.ConceptId && x.UserId == activity.UserId && x.ActivityDate.Date == activity.ActivityDate.Date && x.Status == CommonStatus.Enabled);
            if (existingActivity != null)
            {
                return await _db.UserConceptActivities.Where(x => x.Id == existingActivity.Id)
                        .Set(x => x.Comment, activity.Comment)
                        .Set(x => x.ActivityDate, activity.ActivityDate)
                        .Set(x => x.Duration, activity.Duration)
                        .Set(x => x.ConceptStateId, activity.ConceptStateId)
                        .Set(x => x.ModifiedByUserId, activity.ModifiedByUserId)
                        .Set(x => x.ModifiedTime, DateTime.UtcNow)
                        .UpdateAsync() > 0;

            }
            return await _db.InsertAsync(activity) > 0;
        }

        public async Task<StudySessionDto> GetStudySessionDetailsAsync(int conceptId, int userId, DateTime date)
        {
            var sessionDetails = await (from concept in _db.Concepts.Where(c => c.Id == conceptId && c.Status == CommonStatus.Enabled)
                                        from category in _db.Categories.InnerJoin(c => c.Id == concept.CategoryId && c.Status == CommonStatus.Enabled)
                                        from subject in _db.Subjects.LeftJoin(s => s.Id == category.SubjectId && s.Status == CommonStatus.Enabled)
                                        from uca in _db.UserConceptActivities.LeftJoin(x => x.UserId == userId && x.ConceptId == conceptId && x.Status == CommonStatus.Enabled && x.ActivityDate.Date == date.Date)
                                        select new StudySessionDto
                                        {
                                            ConceptId = concept.Id,
                                            ConceptName = concept.Name,
                                            ConceptDescription = concept.Description,
                                            CategoryId = category.Id,
                                            CategoryName = category.Name,
                                            ConceptStateId = uca != null ? uca.ConceptStateId : 0,
                                            RemainingDuration = concept.Duration - (uca != null ? uca.Duration : 0),
                                            Comment = uca != null ? uca.Comment : null,
                                            SubjectId = subject != null ? subject.Id : null,
                                            SubjectName = subject != null ? subject.Name : null
                                        }).FirstOrDefaultAsync();

            return sessionDetails;
        }
    }
}
