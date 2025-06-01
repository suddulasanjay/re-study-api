using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Utility.Constants;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI.Services.Operation
{
    public class HomeService : IHomeService
    {
        private readonly AppDBContext _db;
        private readonly ICurrentSessionHelper _currentSessionHelper;

        public HomeService(AppDBContext db, ICurrentSessionHelper currentSessionHelper)
        {
            _db = db;
            _currentSessionHelper = currentSessionHelper;
        }
        public async Task<List<AgendaDto>> GetAgendaAsync()
        {
            var session = _currentSessionHelper.GetCurrentSession();
            if (session == null)
                return new List<AgendaDto>();

            var today = DateTime.UtcNow.Date;

            // Step 1: Fetch possible eligible concepts
            var rawConcepts = await (
                from usercategory in _db.UserCategories.Where(uc => uc.UserId == session.UserId && uc.Status == CommonStatus.Enabled)
                from category in _db.Categories.InnerJoin(c => c.Id == usercategory.CategoryId && c.Status == CommonStatus.Enabled)
                from concept in _db.Concepts.InnerJoin(c => c.CategoryId == category.Id && c.Status == CommonStatus.Enabled)
                from subject in _db.Subjects.LeftJoin(s => s.Id == category.SubjectId && s.Status == CommonStatus.Enabled)
                from studies in _db.UserConceptActivities.LeftJoin(u => u.UserId == session.UserId && u.ActivityDate.Date == today && u.ConceptId == concept.Id && u.Status == CommonStatus.Enabled)
                select new
                {
                    concept,
                    subject,
                    studies
                }).ToListAsync();

            // Step 2: Filter by ScheduledDate and RepetitionGap logic in memory
            var result = rawConcepts.Where(x => x.concept.ScheduledDate.Date == today || (x.concept.ScheduledDate.Date < today && x.concept.RepetitionGap > 0 && (today - x.concept.ScheduledDate.Date).Days % x.concept.RepetitionGap == 0))
                                    .Select(x => new AgendaDto
                                    {
                                        ConceptId = x.concept.Id,
                                        ConceptName = x.concept.Name,
                                        SubjectId = x.subject?.Id,
                                        SubjectName = x.subject?.Name,
                                        ConceptStatus = x.studies?.ConceptStateId ?? 0,
                                        ConceptDuration = x.concept.Duration
                                    })
                                    .ToList();
            return result;
        }

    }
}
