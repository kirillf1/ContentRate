using ContentRate.Domain.Rooms;

namespace ContentRate.Application.Contracts.Content
{
    public class ContentDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public ContentType ContentType { get; set; }
        public string Path { get; set; } = default!;
        public List<ContentRating> Ratings { get; set; } = new();
    }
}
