namespace ContentRate.Domain.Notifications
{
    public class Notification
    {
        public Notification(Guid id, string description,TargetUser targetUser,  bool isWatched = false)
        {
            Id = id;
            IsWatched = isWatched;
            Description = description;
            TargetUser = targetUser;
        }

        public Guid Id { get; }
        public bool IsWatched { get; set; }
        public string Description { get; set; }
        public TargetUser TargetUser { get; }
    }
}
