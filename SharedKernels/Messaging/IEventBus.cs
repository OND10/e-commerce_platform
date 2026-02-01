using System.Threading;
using System.Threading.Tasks;

namespace SharedKernels.Messaging
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}
