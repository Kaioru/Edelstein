using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Services
{
    public class NodeService<TState> : NodeLocal<TState>, IHostedService
        where TState : INodeState
    {
        public NodeService(
            TState state,
            ICacheClient cache,
            IMessageBus bus
        ) : base(state, cache, bus)
        {
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            Start();
            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.CompletedTask;
        }
    }
}