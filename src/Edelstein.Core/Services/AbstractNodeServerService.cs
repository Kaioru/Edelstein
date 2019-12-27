using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;
using Edelstein.Network.Transport;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Services
{
    public class AbstractNodeServerService<TState> : AbstractNodeService<TState>
        where TState : IServerNodeState
    {
        private readonly Server _server;

        public AbstractNodeServerService(
            TState state,
            ICacheClient cache,
            IMessageBus bus,
            Server server
        ) : base(state, cache, bus)
            => _server = server;

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _server.Start(State.Host, State.Port);
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _server.Stop();
            await base.StopAsync(cancellationToken);
        }
    }
}