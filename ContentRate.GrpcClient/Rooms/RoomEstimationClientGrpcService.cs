using Ardalis.Result;
using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using ContentRate.ClientProtos;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.Protos;

namespace ContentRate.GrpcClient.Rooms
{
    public class RoomEstimationClientGrpcService : IRoomEstimationService
    {
        private readonly RoomEstimateService.RoomEstimateServiceClient client;

        public RoomEstimationClientGrpcService(RoomEstimateService.RoomEstimateServiceClient client)
        {
            this.client = client;
        }

        public Task<Result> EndEstimation(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> EstimateContent(ContentEstimate contentEstimate)
        {
            try
            {
                await client.EstimateContentAsync(ContentConverter.ConverContentEstimateToGrpc(contentEstimate));
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
                await client.LeaveRoomAsync(new RoomExitGrpc
                {
                    RoomId = exit.RoomId.ToString(),
                    UserId = exit.AssessorId.ToString(),
                });
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
