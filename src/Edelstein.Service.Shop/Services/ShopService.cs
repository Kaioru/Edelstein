using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Shop.Services
{
    public class ShopService : AbstractMigrateableService<ShopServiceInfo>
    {
        public IDataStore DataStore { get; }
        
        public ShopService(
            IOptions<ShopServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDataStore dataStore
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = dataStore;
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new ShopSocket(channel, seqSend, seqRecv, this);
    }
}