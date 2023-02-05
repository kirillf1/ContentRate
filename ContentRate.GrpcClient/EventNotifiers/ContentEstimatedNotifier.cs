using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Events;

namespace ContentRate.GrpcClient.EventNotifiers
{
    internal class ContentEstimatedNotifier : EventNotifier<ContentEstimate>
    {
        public ContentEstimatedNotifier(ContentRateEventHandler<ContentEstimate> handler) : base(handler)
        {
        }
    }
}
