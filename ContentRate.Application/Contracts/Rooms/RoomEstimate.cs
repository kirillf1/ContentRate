using ContentRate.Application.Contracts.Content;

namespace ContentRate.Application.Contracts.Rooms
{
    public class RoomEstimate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public List<ContentDetails> Content { get; set; } = new();
        public List<AssessorTitle> Assessors { get; set; } = new();
        public Guid CreatorId { get; set; }
        
    }
}
