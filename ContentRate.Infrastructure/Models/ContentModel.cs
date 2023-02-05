using ContentRate.Domain.Rooms;
using System.Text.Json.Serialization;

namespace ContentRate.Infrastructure.Models
{
    public class ContentModel
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public RoomModel Room { get; set; }
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public ContentType ContentType { get; set; }
        public string Path { get; set; }
        public List<RatingModel> Ratings { get; set; }
    }
}
