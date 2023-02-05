using ContentRate.Application.Contracts.Users;

namespace ContentRate.Application.Contracts.Rooms
{
    public class RoomTitle
    {
        public string Name { get; set; } = default!;
        public UserTitle Creator { get; set; } = default!;
        public Guid Id { get; set; }
        public int AssessorCount { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
