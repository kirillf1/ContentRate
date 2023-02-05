namespace ContentRate.Infrastructure.Models
{
    public class RatingModel
    {
        public Guid ContentId { get; set; }
        public ContentModel Content { get; set; }
        public Guid UserId { get; set; }
        public double Value { get; set; }
    }
}
