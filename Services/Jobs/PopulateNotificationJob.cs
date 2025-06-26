using ReStudyAPI.Interfaces.Jobs;
using ReStudyAPI.Interfaces.Operation;

namespace ReStudyAPI.Services.Jobs
{
    public class PopulateNotificationJob : IPopulateNotificationJob
    {
        private readonly INotificationService _notificationService;

        public PopulateNotificationJob(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task RunAsync()
        {
            await _notificationService.PopulateScheduledAndMissedNotificationsAsync();
        }
    }
}
