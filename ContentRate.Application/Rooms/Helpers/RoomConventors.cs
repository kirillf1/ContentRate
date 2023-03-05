using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Domain.Rooms;

namespace ContentRate.Application.Rooms.Helpers
{
    public static class RoomConventors
    {
        public static RoomEstimate ConvertRoomToEstimateRoom(Room room)
        {
            return new RoomEstimate
            {
                Assessors = room.Assessors.Select(a => new Contracts.Rooms.AssessorTitle
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsMock = a.IsMockAssessor
                }).ToList(),
                Content = room.ContentList.Select(c => new ContentDetails
                {
                    Id = c.Id,
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Path = c.Path,
                    Ratings = c.Ratings.Select(r => new ContentRating { Value = r.Value, AssessorId = r.AssessorId }).ToList()
                }).ToList(),
                CreatorId = room.RoomDetails.CreatorId,
                Id = room.Id,
                Name = room.Name,
            };
        }
    }
}
