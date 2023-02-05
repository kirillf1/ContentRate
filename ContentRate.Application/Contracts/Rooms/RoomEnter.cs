namespace ContentRate.Application.Contracts.Rooms
{
    public class RoomEnter
    {
        public Guid AssessorId { get; set; }
        public Guid RoomId { get; set; }
        public string? Password { get; set; }
    }
}
