using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Network;
using Edelstein.Service.Login.Sockets;
using Foundatio.Caching;
using Foundatio.Lock;
using Marten;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login
{
    public class WvsLogin : AbstractMigrateableService<LoginServiceInfo>
    {
        public IDocumentStore DocumentStore { get; }
        public ILockProvider LockProvider { get; }

        public WvsLogin(
            IOptions<LoginServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDocumentStore store,
            ILockProvider lockProvider
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DocumentStore = store;
            LockProvider = lockProvider;
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new LoginSocket(channel, seqSend, seqRecv, this);
    }
}