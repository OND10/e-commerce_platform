using MassTransit;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernels.Messaging
{
    public class MassTransitEventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            return _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
