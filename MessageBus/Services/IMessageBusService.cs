namespace MessageBus.Services
{
    public interface IMessageBusService
    {
        Task PublishMessage(object message, string topic_queue_Name);
    }
}
