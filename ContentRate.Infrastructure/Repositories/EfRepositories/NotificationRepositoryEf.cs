using ContentRate.Domain.Notifications;
using ContentRate.Infrastructure.Contexts;
using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ContentRate.Infrastructure.Repositories.EfRepositories
{
    public class NotificationRepositoryEf : INotificationRepository
    {
        private readonly ContentRateDbContext context;

        public NotificationRepositoryEf(ContentRateDbContext context)
        {
            this.context = context;
        }
        public async Task AddNotification(Notification notification)
        {
            var notificationModel = new NotificationModel
            {
                Id = notification.Id,
                Description = notification.Description,
                IsWatched = notification.IsWatched,
                UserId = notification.TargetUser.Id
            };
            await context.Notifications.AddAsync(notificationModel);
        }

        public async Task DeleteNotification(Guid id)
        {
            var notificationForDelete = await context.Notifications.SingleAsync(c => c.Id == id);
            context.Notifications.Remove(notificationForDelete);
        }

        public async Task<Notification> GetNotificationById(Guid id)
        {
            var notificationModel = await context.Notifications.Include(c=>c.User)
                .SingleAsync(c => c.Id == id);
            return ConvertModelToNotification(notificationModel);
        }

        public async Task<IEnumerable<Notification>> GetNotificationByUserId(Guid userId)
        {
            return await context.Notifications.Include(c=>c.User)
                .Where(c=> c.UserId == userId).Select(c=>ConvertModelToNotification(c)).ToListAsync();
        }
        private static Notification ConvertModelToNotification(NotificationModel notificationModel)
        {
            var user = notificationModel.User;
            return new Notification(notificationModel.Id, notificationModel.Description,
                new TargetUser(user.Id, user.Name));
        }
        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateNotification(Notification notification)
        {
            var notificationModel = context.Notifications.Local.FirstOrDefault(n => n.Id == notification.Id)
                ?? await context.Notifications.SingleAsync(c => c.Id == notification.Id);
            notificationModel.Description = notification.Description;
            notificationModel.IsWatched = notification.IsWatched;
            context.Notifications.Update(notificationModel);
        }
    }
}
