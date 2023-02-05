namespace ContentRate.Domain.Rooms
{
    public class RoomDetails
    {
        public RoomDetails(Guid creatorId,bool isPrivate,string? password = null)
        {
            CreatorId = creatorId;
            IsPrivate = isPrivate;
            Password = password;
        }
        public Guid CreatorId { get; }
        public bool IsPrivate { get; set; }
        public string? Password { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
