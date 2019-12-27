using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Services
{
    public abstract class AbstractNodeService<TState> : NodeLocal<TState>, IHostedService
        where TState : INodeState
    {
        protected AbstractNodeService(
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