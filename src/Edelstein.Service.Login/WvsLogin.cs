using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Network;
using Edelstein.Service.Login.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login
{
    public class WvsLogin : AbstractPeerServerService<LoginServiceInfo>
    {
        public WvsLogin(
            IOptions<LoginServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBus messageBus
        ) : base(info.Value, cacheClient, messageBus)
        {
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new LoginSocket(channel, seqSend, seqRecv, this);
    }
}