namespace ContentRate.Application.Contracts.Rooms
{
    public class RoomUpdate : RoomEstimate
    {
        public string? Password { get; set; }
        public bool IsPrivate { get; set; }
    }
}
