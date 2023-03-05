using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Events;

namespace ContentRate.GrpcClient.EventNotifiers
{
    internal class ContentEstimatedListener : EventNotifier<ContentEstimate>
    {
        public ContentEstimatedListener(ContentRateEventHandler<ContentEstimate> handler) : base(handler)
        {
        }
    }
}
