using Ardalis.Result;
using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Domain.Rooms;

namespace ContentRate.Application.Rooms
{
    public class RoomEstimationService : IRoomEstimationService
    {
        private readonly IContentRepository contentRepository;
        private readonly IRoomRepository roomRepository;
        
        public RoomEstimationService(IContentRepository contentRepository,IRoomRepository roomRepository)
        {
            this.contentRepository = contentRepository;
            this.roomRepository = roomRepository;
        }
        public Task<Result> EndEstimation(Guid roomId)
        {

            throw new NotImplementedException();
        }
        public async Task<Result> EstimateContent(ContentEstimate contentEstimate)
        {
            try
            {
                var content = await contentRepository.GetById(contentEstimate.ContentId);
                content.ChangeRating(new Rating(contentEstimate.AssessorId, contentEstimate.NewValue));
                await contentRepository.UpdateContent(content);
                await contentRepository.SaveChanges();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result> LeaveRoom(RoomExit exit)
        {
            try
            {
                var room = await roomRepository.GetRoomById(exit.RoomId);
                room.AssessorLeave(exit.AssessorId);
                await roomRepository.UpdateRoom(room);
                await roomRepository.SaveChanges();
                return Result.Success();
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
