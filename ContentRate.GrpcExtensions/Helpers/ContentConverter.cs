using ContentRate.Application.Contracts.Content;
using ContentRate.Domain.Rooms;
using ContentRate.Protos;

namespace ContentRate.GrpcExtensions.Helpers
{
    public static class ContentConverter
    {
        public static ContentDetails ConvertContentGrpcToDetails(ContentDetailsGrpc content)
        {
            return new ContentDetails
            {
                ContentType = (ContentType)content.ContentType,
                Id = Guid.Parse(content.Id),
                Name = content.Name,
                Path = content.Path,
                Ratings = content.Ratings.Select(r =>
                                        new ContentRating
                                        {
                                            AssessorId = Guid.Parse(r.AssessorId),
                                            Value = r.Value
                                        }).ToList(),
            };
        }
        public static ContentDetailsGrpc ConvertContentDetailsToGrpc(ContentDetails content)
        {
            var contentGrpc = new ContentDetailsGrpc
            {
                ContentType = (uint)content.ContentType,
                Id = content.Id.ToString(),
                Name = content.Name,
                Path = content.Path
            };
            contentGrpc.Ratings.AddRange(content.Ratings.Select(c => new RatingGrpc
            {
                AssessorId = c.AssessorId.ToString(),
                Value = c.Value,
            }));
            return contentGrpc;
        }
        public static ContentEstimate ConvertGrpcToContentEstimate(ContentEstimateGrpc contentGrpc)
        {
            return new ContentEstimate
            {
                AssessorId = Guid.Parse(contentGrpc.AssessorId),
                ContentId = Guid.Parse(contentGrpc.ContentId),
                NewValue = contentGrpc.NewValue,
                RoomId = Guid.Parse(contentGrpc.RoomId),
            };
        }
        public static ContentEstimateGrpc ConverContentEstimateToGrpc(ContentEstimate content)
        {
            return new ContentEstimateGrpc
            {
                AssessorId = content.AssessorId.ToString(),
                ContentId = content.ContentId.ToString(),
                NewValue = content.NewValue,
                RoomId = content.RoomId.ToString(),
            };
        }
    }
}
