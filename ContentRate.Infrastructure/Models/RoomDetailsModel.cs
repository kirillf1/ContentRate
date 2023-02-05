namespace ContentRate.Infrastructure.Models
{
    public class RoomDetailsModel
    {
        public Guid CreatorId { get; set; }
        public bool IsPrivate { get; set; }
        public string? Password { get; set; }
        private DateTime creationTime;
        public DateTime CreationTime { get => creationTime; set => creationTime = value; }
    }
}
