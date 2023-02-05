namespace ContentRate.Infrastructure.Models
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public bool IsWatched { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public UserModel User { get; set; }
    }
}
