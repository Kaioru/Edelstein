using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Shop.Commodities;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Shop.Services
{
    public class ShopService : AbstractMigrateableService<ShopServiceInfo>
    {
        public IDataStore DataStore { get; }
        public ITemplateManager TemplateManager { get; }
        public CommodityManager CommodityManager { get; }

        public ShopService(
            IOptions<ShopServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDataStore dataStore,
            ITemplateManager templateManager
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = dataStore;
            TemplateManager = templateManager;
            CommodityManager = new CommodityManager(TemplateManager);
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new ShopSocket(channel, seqSend, seqRecv, this);
    }
}