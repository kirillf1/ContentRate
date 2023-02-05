using ContentRate.Domain.Notifications;
using ContentRate.Infrastructure.Repositories.EfRepositories;

namespace ContentRate.UnitTests.EfContextTests
{
    public class NotificationRepositoryEfTests
    {
        [Fact]
        public async Task AddNotification_NewNotification_ShouldAddToDb()
        {
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            INotificationRepository notificationRepository = new NotificationRepositoryEf(context);
            var user = new Infrastructure.Models.UserModel { Name = "", Id = Guid.NewGuid(), Password = "fafsafafa" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var notification = new Notification(Guid.NewGuid(), "test", new TargetUser(user.Id, user.Name));
            await notificationRepository.AddNotification(notification);
            await notificationRepository.SaveChanges();
           
            var notificationFromDb = context.Notifications.First(c=>c.Id == notification.Id);
            Assert.Equal(notification.Description,notificationFromDb.Description);
            Assert.Equal(notification.TargetUser.Id, notificationFromDb.UserId);
        }
    }
}
