using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface IDashboardService
    {
        Task<List<UserConceptActivityDto>> GetUserConceptActivitiesAsync(DateTime startDate, DateTime endDate);
    }
}
