using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Messaging
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}
