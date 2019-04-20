using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Network;
using Edelstein.Network.Transport;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed
{
    public abstract class AbstractPeerServerService<TInfo> : AbstractPeerService<TInfo>, ISocketFactory
        where TInfo : ServerServiceInfo
    {
        protected readonly Server Server;

        public AbstractPeerServerService(
            TInfo info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info, cacheClient, messageBusFactory)
        {
            Server = new Server(this);
        }

        public override async Task OnStart()
        {
            await Server.Start(Info.Host, Info.Port);
            await base.OnStart();
        }

        public override async Task OnStop()
        {
            await base.OnStop();
            await Server.Stop();
        }

        public abstract ISocket Build(IChannel channel, uint seqSend, uint seqRecv);
    }
}