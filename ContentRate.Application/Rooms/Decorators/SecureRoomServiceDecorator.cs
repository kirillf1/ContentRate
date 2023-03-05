using Ardalis.Result;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms.Helpers;
using ContentRate.Application.Users;
using ContentRate.Domain.Rooms;

namespace ContentRate.Application.Rooms.Decorators
{
    public class SecureRoomServiceDecorator : IRoomService
    {
        private readonly IUserContext userContext;
        private readonly IRoomService roomService;
        private readonly IRoomRepository roomRepository;

        public SecureRoomServiceDecorator(IUserContext userContext, IRoomService roomService, IRoomRepository roomRepository)
        {
            this.userContext = userContext;
            this.roomService = roomService;
            this.roomRepository = roomRepository;
        }
        public async Task<Result> CreateRoom(RoomUpdate roomCreate)
        {
            try
            {
                var user = await userContext.TryGetCurrentUser();
                if (user is null || roomCreate.CreatorId != user.Id)
                    return Result.Forbidden();
                return await roomService.CreateRoom(roomCreate);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result> DeleteRoom(Guid roomId)
        {
            var user = await userContext.TryGetCurrentUser();
            if (user is null)
                return Result.Forbidden();
            try
            {
                var roomDetails = await roomRepository.GetRoomDetailsById(roomId);
                if (roomDetails.CreatorId != user.Id)
                    return Result.Forbidden();
                return await roomService.DeleteRoom(roomId);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        // как-то не очень получилось в данном случае, подумать над исправлением
        public async Task<Result<RoomEstimate>> JoinRoom(RoomEnter enter)
        {
            try
            {
                var user = await userContext.TryGetCurrentUser();
                if (user is null)
                    return Result.Forbidden();
                var roomDetails = await roomRepository.GetRoomDetailsById(enter.RoomId);
                if (roomDetails.CreatorId != user.Id)
                    return await roomService.JoinRoom(enter);
                var room = await roomRepository.GetRoomById(enter.RoomId);
                return RoomConventors.ConvertRoomToEstimateRoom(room);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<RoomUpdate>> OpenRoomToUpdate(Guid roomId)
        {
            try
            {
                var user = await userContext.TryGetCurrentUser();
                if (user is null)
                    return Result.Forbidden();
                var roomDetails = await roomRepository.GetRoomDetailsById(roomId);
                if (roomDetails.CreatorId == user.Id)
                    return await roomService.OpenRoomToUpdate(roomId);
                return Result.Forbidden();
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result> UpdateRoom(RoomUpdate roomUpdate)
        {
            try
            {
                var user = await userContext.TryGetCurrentUser();
                if (user is null)
                    return Result.Forbidden();
                var roomDetails = await roomRepository.GetRoomDetailsById(roomUpdate.Id);
                if (roomDetails.CreatorId == user.Id)
                    return await roomService.UpdateRoom(roomUpdate);
                return Result.Forbidden();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
