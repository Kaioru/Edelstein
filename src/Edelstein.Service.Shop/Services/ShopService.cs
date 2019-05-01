using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Network;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Shop.Services
{
    public class ShopService : AbstractMigrateableService<ShopServiceInfo>
    {
        public ShopService(
            IOptions<ShopServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new ShopSocket(channel, seqSend, seqRecv, this);
    }
}