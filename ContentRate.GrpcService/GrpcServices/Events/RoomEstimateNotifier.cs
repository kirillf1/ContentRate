using ContentRate.Protos;

namespace ContentRate.GrpcService.GrpcServices.Events
{
    public class RoomEstimateNotifier
    {
        private readonly RoomEstimateEventListenerStorage storage;

        public RoomEstimateNotifier(RoomEstimateEventListenerStorage storage)
        {
            this.storage = storage;
        }
        public async Task NotifyAllRoom(Guid roomId,RoomEstimateEventGrpc roomEstimateEventGrpc, Guid? ingnoreSender)
        {
            var users = storage.TryGetRoomConnections(roomId);
            if (ingnoreSender.HasValue)
                users = users.Where(c => c.UserId != ingnoreSender.Value);
            foreach (var connection in users.Select(c=>c.Connection))
            {
                await connection.WriteAsync(roomEstimateEventGrpc);
            }
        }
    }
}
