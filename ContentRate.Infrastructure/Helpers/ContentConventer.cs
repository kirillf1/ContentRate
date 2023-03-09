using ContentRate.Domain.Rooms;
using ContentRate.Infrastructure.Models;

namespace ContentRate.Infrastructure.Helpers
{
    internal static class ContentConventer
    {
        public static ContentModel ConvertContentToModel(Content content, Guid roomId)
        {
            return new ContentModel
            {
                ContentType = content.ContentType,
                Id = content.Id,
                Name = content.Name,
                Path = content.Path,
                Ratings = content.Ratings.Select(r => RatingConverter.ConvertRatingToModel(r,content.Id)).ToList(),
                RoomId = roomId
            };
        }
        public static Content ConvertModelToContent(ContentModel contentModel)
        {
            return new Content(contentModel.Id, contentModel.Name, contentModel.ContentType, 
                contentModel.Path,contentModel.Ratings.Select(RatingConverter.ConvertModelToRating));
        }
        public static void UpdateContentModel(ContentModel contentModel, Content contentUpdate)
        {
            contentModel.ContentType = contentUpdate.ContentType;
            contentModel.Path = contentUpdate.Path;
            UpdateRating(contentUpdate, contentModel);
        }
        private static void UpdateRating(Content contentUpdate, ContentModel contentModel)
        {
            foreach (var ratingForDelete in contentModel.Ratings.Where(r => !contentUpdate.Ratings.Any(
                                c => c.AssessorId == r.UserId)).ToList())
            {
                contentModel.Ratings.Remove(ratingForDelete);
            }
            var ratingForAdd = contentUpdate.Ratings.Where(c => !contentModel.Ratings.Any(n => n.UserId == c.AssessorId)).ToList();
            foreach (var newRating in ratingForAdd)
            {
                contentModel.Ratings.Add(new RatingModel
                {
                    ContentId = contentModel.Id,
                    UserId = newRating.AssessorId,
                    Value = newRating.Value
                });
            }
            foreach (var ratingUpdate in contentUpdate.Ratings.Where(c => !ratingForAdd.Any(r => r.AssessorId == c.AssessorId)))
            {
                var ratingModel = contentModel.Ratings.First(c => c.UserId == ratingUpdate.AssessorId);
                ratingModel.Value = ratingUpdate.Value;
            }
        }
    }
}
