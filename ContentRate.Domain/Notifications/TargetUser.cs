namespace ContentRate.Domain.Notifications
{
    public class TargetUser
    {
        public TargetUser(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}
