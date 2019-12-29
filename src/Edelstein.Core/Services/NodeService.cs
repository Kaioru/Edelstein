using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Services
{
    public class NodeService<TState> : CachedNodeLocal<TState>, IHostedService
        where TState : INodeState
    {
        public NodeService(
            TState state,
            ICacheClient cache,
            IMessageBusFactory busFactory
        ) : base(state, cache, busFactory)
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