using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface INotificationService
    {
        Task PopulateScheduledAndMissedNotificationsAsync();
        Task<List<NotificationDto>> GetNotificationsAsync();
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAsUnReadAsync(int notificationId);
    }
}
