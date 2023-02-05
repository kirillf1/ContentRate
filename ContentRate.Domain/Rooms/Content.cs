namespace ContentRate.Domain.Rooms
{
    public class Content
    {
        public Content(Guid id, string name, ContentType contentType, string path, IEnumerable<Rating> ratings)
        {
            Id = id;
            Name = name;
            ContentType = contentType;
            Path = path;
            _ratings = new(ratings);
        }
        public IReadOnlyCollection<Rating> Ratings { get => _ratings; }
        public Guid Id { get; }
        public string Name { get; set; }
        public ContentType ContentType { get; set; }
        public string Path { get; set; }
        private List<Rating> _ratings;
        public void AddRating(Rating rating)
        {
            if (!_ratings.Any(c => c.AssessorId == rating.AssessorId))
                _ratings.Add(rating);
        }
        public void ChangeRating(Rating rating)
        {
            var oldRating = _ratings.Find(c => c.AssessorId == rating.AssessorId);
            if (oldRating is null)
                return;
            oldRating.ChangeRating(rating.Value);
        }
        public void DeleteRatingByUser(Guid userId)
        {
            var rating = _ratings.Find(r => r.AssessorId == userId);
            if (rating != null)
                _ratings.Remove(rating);
        }
        public double GetAverageRating()
        {
           return Ratings.Average(c => c.Value);
        }
    }
}
