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
            int userId = await _currentSessionHelper.GetUserId(session);

            return await (from uca in _db.UserConceptActivities.Where(x => x.UserId == userId && x.Status == CommonStatus.Enabled && x.ActivityDate.Date >= startDate.Date && x.ActivityDate.Date <= endDate.Date)
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

        public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync()
        {
            var session = _currentSessionHelper.GetCurrentSession();

            int userId = await _currentSessionHelper.GetUserId(session);
            return await (from uca in _db.UserConceptActivities.Where(x => x.UserId == userId && x.Status == CommonStatus.Enabled)
                          from concept in _db.Concepts.InnerJoin(c => c.Id == uca.ConceptId && c.Status == CommonStatus.Enabled)
                          from category in _db.Categories.InnerJoin(cat => cat.Id == concept.CategoryId && cat.Status == CommonStatus.Enabled)
                          from subject in _db.Subjects.LeftJoin(sub => sub.Id == category.SubjectId && sub.Status == CommonStatus.Enabled)
                          from state in _db.ConceptStates.InnerJoin(st => st.Id == uca.ConceptStateId && st.Status == CommonStatus.Enabled)
                          select new RecentActivityDto
                          {
                              Concept = concept.Name,
                              Subject = subject != null ? subject.Name : null,
                              ActivityDate = uca.ActivityDate,
                              Status = state.Id
                          })
                          .OrderByDescending(x => x.ActivityDate)
                          .Take(3)
                          .ToListAsync();
        }

        public async Task<List<StreakDayDto>> GetMonthlyStreakAsync(int year, int month)
        {
            var session = _currentSessionHelper.GetCurrentSession();

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1); // Last day of month
            int userId = await _currentSessionHelper.GetUserId(session);

            var streaks = await (from uca in _db.UserConceptActivities
                                 where uca.UserId == userId
                                       && uca.Status == CommonStatus.Enabled
                                       && uca.ActivityDate.Date >= startDate.Date
                                       && uca.ActivityDate.Date <= endDate.Date
                                 group uca by uca.ActivityDate.Date into g
                                 select new StreakDayDto
                                 {
                                     Date = g.Key,
                                     ActivityCount = g.Count()
                                 }).ToListAsync();

            return streaks;
        }

        public async Task<StreakDetailsDto> GetStreakDetailsAsync()
        {
            var session = _currentSessionHelper.GetCurrentSession();
            int userId = await _currentSessionHelper.GetUserId(session);

            // 1. Fetch all active activity dates for user
            var activityDates = await _db.UserConceptActivities.Where(x => x.UserId == userId && x.Status == CommonStatus.Enabled)
                                .Select(x => x.ActivityDate.Date).Distinct().ToListAsync();

            if (activityDates.Count == 0)
                return new StreakDetailsDto();

            // 2. Sort and process
            var sortedDates = activityDates.OrderBy(d => d).ToList();
            int currentStreak = 1;
            int longestStreak = 1;

            // Max Streak
            for (int i = 1; i < sortedDates.Count; i++)
            {
                if ((sortedDates[i] - sortedDates[i - 1]).TotalDays == 1)
                {
                    currentStreak++;
                    longestStreak = Math.Max(longestStreak, currentStreak);
                }
                else
                {
                    currentStreak = 1;
                }
            }

            // Current Streak
            var today = DateTime.UtcNow.Date;
            int currentStreakCount = 0;

            for (int i = sortedDates.Count - 1; i >= 0; i--)
            {
                var expected = today.AddDays(-currentStreakCount);
                if (sortedDates[i] == expected)
                {
                    currentStreakCount++;
                }
                else if (sortedDates[i] < expected)
                {
                    break;
                }
            }

            return new StreakDetailsDto
            {
                MaximumStreak = longestStreak,
                CurrentStreak = currentStreakCount
            };
        }
    }
}
