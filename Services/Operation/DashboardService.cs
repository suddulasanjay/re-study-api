using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Utility.Constants;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI.Services.Operation
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDBContext _db;
        private readonly ICurrentSessionHelper _currentSessionHelper;

        public DashboardService(AppDBContext db, ICurrentSessionHelper currentSessionHelper)
        {
            _db = db;
            _currentSessionHelper = currentSessionHelper;
        }

        public async Task<List<UserConceptActivityDto>> GetUserConceptActivitiesAsync(DateTime startDate, DateTime endDate)
        {
            var session = _currentSessionHelper.GetCurrentSession();
            if (session == null)
            {
                return new List<UserConceptActivityDto>();
            }
            else
            {

                return await (from uca in _db.UserConceptActivities.Where(x => x.UserId == session.UserId && x.Status == CommonStatus.Enabled && x.ActivityDate.Date >= startDate.Date && x.ActivityDate.Date <= endDate.Date)
                              from concept in _db.Concepts.InnerJoin(x => x.Id == uca.ConceptId && x.Status == CommonStatus.Enabled)
                              from category in _db.Categories.InnerJoin(x => x.Id == concept.CategoryId && x.Status == CommonStatus.Enabled)
                              from subject in _db.Subjects.LeftJoin(x => x.Id == category.SubjectId && x.Status == CommonStatus.Enabled)
                              select new UserConceptActivityDto
                              {
                                  ConceptId = concept.Id,
                                  CategoryId = category.Id,
                                  SubjectId = subject != null ? subject.Id : (int?)null,
                                  ProgressId = uca.ConceptStateId,
                                  ConceptName = concept.Name,
                                  CategoryName = category.Name,
                                  SubjectName = subject != null ? subject.Name : null,
                                  ActivityDate = uca.ActivityDate
                              }).ToListAsync();

            }
        }

    }
}
