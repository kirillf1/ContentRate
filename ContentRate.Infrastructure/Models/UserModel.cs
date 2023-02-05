using System.Text.Json.Serialization;

namespace ContentRate.Infrastructure.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get;  set; }
        public bool IsMockUser { get; set; }
        public List<NotificationModel> Notifications { get; set; }
        [JsonIgnore]
        public List<RoomModel> Rooms { get; set; }
    }
}
