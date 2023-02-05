namespace ContentRate.Domain.Notifications
{
    public interface INotificationRepository
    {
        public Task<Notification> GetNotificationById(Guid id);
        public Task<IEnumerable<Notification>> GetNotificationByUserId(Guid userId);
        public Task AddNotification(Notification notification);
        public Task UpdateNotification(Notification notification);
        public Task DeleteNotification(Guid id);
        public Task SaveChanges(CancellationToken cancellationToken = default);
    }
}
