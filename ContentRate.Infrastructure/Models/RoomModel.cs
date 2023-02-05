namespace ContentRate.Infrastructure.Models
{
    public class RoomModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserModel> Users { get; set; } = new();
        public RoomDetailsModel RoomDetails { get; set; }
        public List<ContentModel> ContentList { get; set; } = new();
       
    }
}
