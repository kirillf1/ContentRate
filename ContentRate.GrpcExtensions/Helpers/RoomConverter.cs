using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Protos;
using Google.Protobuf.WellKnownTypes;

namespace ContentRate.GrpcExtensions.Helpers
{
    public static class RoomConverter
    {
        public static RoomEnterGrpc ConvertRoomEnterToGrpc(RoomEnter roomEnter)
        {
            return new RoomEnterGrpc
            {
                Password = roomEnter.Password,
                RoomId = roomEnter.RoomId.ToString(),
                UserId = roomEnter.AssessorId.ToString(),
            };
        } 
        public static RoomUpdateGrpc CreateRoomUpdateGrpc(RoomUpdate room)
        {
            var roomEstimate = CreateRoomEstimateGrpc(room);
            return new RoomUpdateGrpc { Room = roomEstimate, IsPrivate = room.IsPrivate, Password = room.Password };
        }
        public static RoomEstimateGrpc CreateRoomEstimateGrpc(RoomEstimate room)
        {
            var roomEstimateGrpc = new RoomEstimateGrpc
            {
                CreatorId = room.CreatorId.ToString(),
                Id = room.Id.ToString(),
                Name = room.Name,
            };
            roomEstimateGrpc.Assessors.AddRange(room.Assessors.Select(a => new AssessorGrpc
            {
                Id = a.Id.ToString(),
                IsMock = a.IsMock,
                Name = a.Name,
            }));
            List<ContentDetailsGrpc> contentDetailsList = new List<ContentDetailsGrpc>();
            foreach (var content in room.Content)
            {
                var contentDetails = ContentConverter.ConvertContentDetailsToGrpc(content);
                contentDetailsList.Add(contentDetails);
            }
            roomEstimateGrpc.Content.AddRange(contentDetailsList);
            return roomEstimateGrpc;
        }

        public static RoomUpdate ConvertGrpcToRoomUpdate(RoomUpdateGrpc request)
        {
            List<ContentDetails> content = ConvertContentGrpcToDetails(request.Room.Content);
            return new RoomUpdate
            {
                Assessors = request.Room.Assessors.Select(u => new AssessorTitle
                {
                    Id = Guid.Parse(u.Id),
                    IsMock = u.IsMock,
                    Name = u.Name,
                }).ToList(),
                Content = content,
                CreatorId = Guid.Parse(request.Room.CreatorId),
                Id = Guid.Parse(request.Room.Id),
                Name = request.Room.Name,
                IsPrivate = request.IsPrivate,
                Password = string.IsNullOrWhiteSpace(request.Password) ? null : request.Password,
            };
        }
        public static RoomEstimate ConvertGrpcToRoomEstimate(RoomEstimateGrpc request)
        {
            List<ContentDetails> content = ConvertContentGrpcToDetails(request.Content);
            return new RoomEstimate
            {
                Assessors = request.Assessors.Select(u => new AssessorTitle
                {
                    Id = Guid.Parse(u.Id),
                    IsMock = u.IsMock,
                    Name = u.Name,
                }).ToList(),
                Content = content,
                CreatorId = Guid.Parse(request.CreatorId),
                Id = Guid.Parse(request.Id),
                Name = request.Name,               
            };
        }
        public static RoomTitleGrpc ConvertRoomTitleToGrpc(RoomTitle roomTitle)
        {
            var creator = roomTitle.Creator;
            UserTitleGrpc userTitleGrpc = UserConverter.ConvertToUserTitleGrpc(creator);
            return new RoomTitleGrpc
            {
                AssessorCount = (uint)roomTitle.AssessorCount,
                CreationTime = Timestamp.FromDateTime(DateTime.SpecifyKind(roomTitle.CreationTime,DateTimeKind.Utc)),
                Creator = userTitleGrpc,
                Id = roomTitle.Id.ToString(),
                Name = roomTitle.Name,
            };
        }
        public static RoomTitle ConvertGrpcToRoomTitle(RoomTitleGrpc roomTitleGrpc)
        {
            return new RoomTitle
            {
                AssessorCount = (int)roomTitleGrpc.AssessorCount,
                CreationTime = roomTitleGrpc.CreationTime.ToDateTime(),
                Creator = UserConverter.ConvertGrpcToUserTitle(roomTitleGrpc.Creator),
                Id = Guid.Parse(roomTitleGrpc.Id),
                Name = roomTitleGrpc.Name,
            };
        }
        private static List<ContentDetails> ConvertContentGrpcToDetails(IEnumerable<ContentDetailsGrpc> content)
        {
            return content.Select(c => ContentConverter.ConvertContentGrpcToDetails(c)).ToList();
        }
    }
}
