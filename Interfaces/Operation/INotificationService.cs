using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface INotificationService
    {
        Task PopulateScheduledAndMissedNotificationsAsync();
        Task<List<NotificationDto>> GetNotificationsAsync();
        Task<bool> MarkAllAsReadAsync();
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsUnReadAsync();
        Task<bool> MarkAsUnReadAsync(int notificationId);
        Task<bool> DeleteAsync(int notificationId);
    }
}
