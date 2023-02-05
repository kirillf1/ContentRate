namespace ContentRate.Application.Events
{
    public delegate Task ContentRateEventHandler<T>(T param);
}
