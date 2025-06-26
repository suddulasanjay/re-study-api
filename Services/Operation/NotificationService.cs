using LinqToDB;
using LinqToDB.Data;
using ReStudyAPI.Data;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Utility.Constants;
using ReStudyAPI.Utility.Enum;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI.Services.Operation
{
    public class NotificationService : INotificationService
    {
        private readonly ICurrentSessionHelper _sessionHelper;
        private readonly AppDBContext _db;

        public NotificationService(ICurrentSessionHelper currentSessionHelper, AppDBContext db)
        {
            _sessionHelper = currentSessionHelper;
            _db = db;
        }

        public async Task<List<NotificationDto>> GetNotificationsAsync()
        {
            var userId = (_sessionHelper.GetCurrentSession())?.UserId;
            var results = await (from n in _db.Notifications.Where(x => x.UserId == userId && x.Status == CommonStatus.Enabled)
                                 from c in _db.Concepts.InnerJoin(x => x.Id == n.ConceptId && x.Status == CommonStatus.Enabled)
                                 select new
                                 {
                                     n.Id,
                                     n.IsRead,
                                     TypeId = n.NotificationTypeId,
                                     ConceptName = c.Name,
                                     n.CreatedTime,
                                 }).OrderByDescending(x => x.CreatedTime).ToListAsync();

            return results.Select(r => new NotificationDto
            {
                Id = r.Id,
                IsRead = r.IsRead,
                Title = r.TypeId == (int)EnumNotificationType.Scheduled ? "Today's Concept" : "Missed Concept",
                Message = r.TypeId == (int)EnumNotificationType.Scheduled
                    ? $"You have '{r.ConceptName}' scheduled for today."
                    : $"You missed the concept '{r.ConceptName}'. Catch up now."
            }).ToList();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var userId = (_sessionHelper.GetCurrentSession())?.UserId;
            var updated = await _db.GetTable<Notification>()
                .Where(n => n.Id == notificationId && n.Status == "E")
                .Set(n => n.IsRead, true)
                .Set(n => n.ModifiedTime, DateTime.UtcNow)
                .Set(n => n.ModifiedByUserId, userId)
                .UpdateAsync();

            return updated > 0;
        }

        public async Task<bool> MarkAsUnReadAsync(int notificationId)
        {
            var userId = (_sessionHelper.GetCurrentSession())?.UserId;
            var updated = await _db.GetTable<Notification>()
                .Where(n => n.Id == notificationId && n.Status == "E")
                .Set(n => n.IsRead, false)
                .Set(n => n.ModifiedTime, DateTime.UtcNow)
                .Set(n => n.ModifiedByUserId, userId)
                .UpdateAsync();

            return updated > 0;
        }

        public async Task PopulateScheduledAndMissedNotificationsAsync()
        {
            var today = DateTime.UtcNow.Date;

            var userIds = await _db.Users.Where(u => u.Status == CommonStatus.Enabled).Select(u => u.Id).ToListAsync();

            var concepts = await _db.Concepts.Where(c => c.Status == CommonStatus.Enabled).Select(c => new { c.Id, c.ScheduledDate }).ToListAsync();

            var todayConceptIds = concepts.Where(c => c.ScheduledDate == today).Select(c => c.Id).ToList();

            var pastConceptIds = concepts.Where(c => c.ScheduledDate < today).Select(c => c.Id).ToList();

            var newNotifications = new List<Notification>();

            foreach (var userId in userIds)
            {
                // SCHEDULED
                foreach (var conceptId in todayConceptIds)
                {
                    bool exists = await _db.GetTable<Notification>().AnyAsync(n =>
                        n.UserId == userId &&
                        n.ConceptId == conceptId &&
                        n.NotificationTypeId == (int)EnumNotificationType.Scheduled &&
                        n.Status == CommonStatus.Enabled);

                    if (!exists)
                    {
                        newNotifications.Add(new Notification
                        {
                            UserId = userId,
                            ConceptId = conceptId,
                            NotificationTypeId = (int)EnumNotificationType.Scheduled,
                            IsRead = false,
                            CreatedTime = DateTime.UtcNow,
                            Status = CommonStatus.Enabled
                        });
                    }
                }

                // MISSED
                foreach (var conceptId in pastConceptIds)
                {
                    var completed = await _db.UserConceptActivities.AnyAsync(a =>
                        a.UserId == userId &&
                        a.ConceptId == conceptId &&
                        a.ConceptStateId == 2 &&
                        a.Status == CommonStatus.Enabled);

                    if (!completed)
                    {
                        bool exists = await _db.GetTable<Notification>().AnyAsync(n =>
                            n.UserId == userId &&
                            n.ConceptId == conceptId &&
                            n.NotificationTypeId == (int)EnumNotificationType.Missed &&
                            n.Status == CommonStatus.Enabled);

                        if (!exists)
                        {
                            newNotifications.Add(new Notification
                            {
                                UserId = userId,
                                ConceptId = conceptId,
                                NotificationTypeId = (int)EnumNotificationType.Missed,
                                IsRead = false,
                                CreatedTime = DateTime.UtcNow,
                                Status = CommonStatus.Enabled
                            });
                        }
                    }
                }
            }

            if (newNotifications.Any())
            {
                await _db.BulkCopyAsync(newNotifications);
            }
        }
    }
}
