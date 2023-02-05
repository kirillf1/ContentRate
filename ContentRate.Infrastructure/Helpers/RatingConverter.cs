using ContentRate.Domain.Rooms;
using ContentRate.Infrastructure.Models;

namespace ContentRate.Infrastructure.Helpers
{
    internal static class RatingConverter
    {
        public static RatingModel ConvertRatingToModel(Rating r, Guid contentId)
        {
            return new RatingModel { UserId = r.AssessorId, Value = r.Value, ContentId = contentId };
        }
        public static Rating ConvertModelToRating(RatingModel rating)
        {
            return new Rating(rating.UserId, rating.Value);
        }
    }
}
