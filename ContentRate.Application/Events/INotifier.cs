namespace ContentRate.Application.Events
{
    public interface INotifier
    {
        public Task StartListenEvents();
        public Task StopListenEvents();
    }
}
