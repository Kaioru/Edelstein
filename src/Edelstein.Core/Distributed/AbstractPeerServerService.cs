using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Distributed.Utils.Messaging;
using Edelstein.Network;
using Edelstein.Network.Transport;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed
{
    public abstract class AbstractPeerServerService<TInfo> : PeerService<TInfo>, ISocketFactory
        where TInfo : ServerServiceInfo
    {
        private readonly Server _server;

        public AbstractPeerServerService(
            TInfo info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info, cacheClient, messageBusFactory)
        {
            _server = new Server(this);
        }

        public override async Task OnStart()
        {
            await _server.Start(Info.Host, Info.Port);
            await base.OnStart();
        }

        public override async Task OnStop()
        {
            await base.OnStop();
            await _server.Stop();
        }

        public abstract ISocket Build(IChannel channel, uint seqSend, uint seqRecv);
    }
}