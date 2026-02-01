using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface IDashboardService
    {
        public Task<List<UserConceptActivityDto>> GetUserConceptActivitiesAsync(DateTime startDate, DateTime endDate);
        public Task<List<RecentActivityDto>> GetRecentActivitiesAsync();
        public Task<List<StreakDayDto>> GetMonthlyStreakAsync(int year, int month);
        public Task<StreakDetailsDto> GetStreakDetailsAsync();

    }
}
