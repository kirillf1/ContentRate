using ContentRate.Application.Events;
using System.Text.Json;

namespace ContentRate.GrpcClient.EventNotifiers
{
    internal interface IEventNotifierBase
    {
        bool TryNotify(Protos.RoomEstimateEventGrpc eventGrpc);
    }
    internal abstract class EventNotifier<TParam> : IEventNotifierBase
    {
        protected readonly ContentRateEventHandler<TParam> handler;

        protected EventNotifier(ContentRateEventHandler<TParam> handler)
        {
            this.handler = handler;
        }
        public virtual bool TryNotify(Protos.RoomEstimateEventGrpc eventGrpc)
        {
            if (handler is null)
                return false;
            try
            {
                var param = JsonSerializer.Deserialize<TParam>(eventGrpc.JsonBody);
                if(param is null) 
                    return false;
                handler.Invoke(param);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
