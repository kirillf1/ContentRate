namespace ContentRate.Domain.Rooms
{
    public class Rating
    {
        public Rating(Guid assessorId, double value)
        {
            AssessorId = assessorId;
            ChangeRating(value);
        }
        public Guid AssessorId { get; }
        public double Value { get; private set; }

        public void ChangeRating(double rating)
        {
            if (!IsValidRating(rating))
                throw new ArgumentException("Rating must be more than zero and less than or equals 10");
           Value = rating;
        }
        private static bool IsValidRating(double rating)
        {
            return rating >= 0 && rating <= 10;
        }
       
    }
}
