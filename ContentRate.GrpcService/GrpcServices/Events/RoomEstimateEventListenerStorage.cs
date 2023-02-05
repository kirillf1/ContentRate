using ContentRate.Protos;
using Grpc.Core;
using System.Collections.Concurrent;

namespace ContentRate.GrpcService.GrpcServices.Events
{
    public record EstimateStorageKey(Guid RoomId, Guid AssessorId);
    public record UserConnection(Guid UserId, IServerStreamWriter<RoomEstimateEventGrpc> Connection);
    public class RoomEstimateEventListenerStorage
    {

        private ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, IServerStreamWriter<RoomEstimateEventGrpc>>> roomConnections = new();
        public bool TryAddConnection(EstimateStorageKey key, IServerStreamWriter<RoomEstimateEventGrpc> writer)
        {
            if (!roomConnections.TryGetValue(key.RoomId, out var connections))
            {
                var isAdded = roomConnections.TryAdd(key.RoomId, new());
                if (!isAdded)
                    return false;
                connections = roomConnections[key.RoomId];
            }
            connections[key.AssessorId] = writer;
            return true;
        }
        public bool TryRemoveConnection(EstimateStorageKey key)
        {
            if (!roomConnections.TryGetValue(key.RoomId, out var connections))
                return true;
            var isLast = connections.Count == 1;
            if (isLast)
                return roomConnections.TryRemove(key.RoomId, out _);
            return connections.TryRemove(key.AssessorId, out _);
        }
        public IServerStreamWriter<RoomEstimateEventGrpc>? TryGetUserConnection(Guid roomId, Guid userId)
        {
            if (!roomConnections.TryGetValue(roomId, out var connections)
                || !connections.TryGetValue(userId, out var connection))
            {
                return null;
            }
            return connection;
        }
        public IEnumerable<UserConnection> TryGetRoomConnections(Guid roomId)
        {
            if (!roomConnections.TryGetValue(roomId, out var connections))
                return Enumerable.Empty<UserConnection>();
            return connections.Select(c=>new UserConnection(c.Key,c.Value));
        }
    }
}
