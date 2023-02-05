namespace ContentRate.Application.Contracts.Rooms
{
    public class AssessorTitle
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsMock { get; set; }
    }
}
