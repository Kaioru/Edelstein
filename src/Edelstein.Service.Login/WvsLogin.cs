using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Network;
using Edelstein.Service.Login.Sockets;
using Foundatio.Caching;
using Marten;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login
{
    public class WvsLogin : AbstractAbstractPeerServerService<LoginServiceInfo>
    {
        public IDocumentStore DocumentStore { get; }

        public WvsLogin(
            IOptions<LoginServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDocumentStore store
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DocumentStore = store;
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new LoginSocket(channel, seqSend, seqRecv, this);
    }
}