namespace ContentRate.Application.Contracts.Content
{
    public class ContentEstimate
    {
        public double NewValue { get; set; }
        public Guid AssessorId { get; set; }
        public Guid RoomId { get; set; }
        public Guid ContentId { get; set; }
    }
}
