using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Network;
using Edelstein.Network.Transport;

namespace Edelstein.Core.Services
{
    public abstract class AbstractNodeServerService<TState> : NodeService<TState>, ISocketAdapterFactory
        where TState : IServerNodeState
    {
        private readonly Server _server;

        public AbstractNodeServerService(
            TState state,
            IMessageBusFactory busFactory
        ) : base(state, busFactory)
        {
            _server = new Server(this, state.Version, state.Patch, state.Locale);
        }

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

        public abstract ISocketAdapter Build(ISocket socket);
    }
}