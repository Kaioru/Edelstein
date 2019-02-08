using System;
using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Migrations;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Shop.Commodity;
using Edelstein.Service.Shop.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Shop
{
    public class WvsShop : AbstractMigrateableService<ShopServiceInfo>
    {
        private IServer Server { get; set; }
        public ITemplateManager TemplateManager { get; }

        public CommodityManager CommodityManager { get; }

        public WvsShop(
            IApplicationLifetime appLifetime,
            ICacheClient cache,
            IMessageBus messageBus,
            IOptions<ShopServiceInfo> info,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : base(appLifetime, cache, messageBus, info, dataContextFactory)
        {
            TemplateManager = templateManager;
            CommodityManager = new CommodityManager(TemplateManager);
        }

        public override Task OnUpdate(DateTime now)
        {
            // TODO
            return Task.CompletedTask;
        }

        protected override async Task OnStarted()
        {
            Server = new Server(new WvsShopSocketFactory(this));

            await base.OnStarted();
            await Server.Start(Info.Host, Info.Port);
        }

        protected override async Task OnStopping()
        {
            await base.OnStopping();
            await Server.Stop();
        }
    }
}