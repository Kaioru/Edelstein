using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Migrations;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Trade.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Service.Trade
{
    public class WvsTrade : AbstractMigrateableService<TradeServiceInfo>
    {
        private IServer Server { get; set; }
        public ITemplateManager TemplateManager { get; }

        public WvsTrade(
            TradeServiceInfo info,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : base(info, cache, messageBus, dataContextFactory)
            => TemplateManager = templateManager;

        public WvsTrade(
            WvsTradeOptions options,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : this(options.Service, cache, messageBus, dataContextFactory, templateManager)
        {
        }

        public override async Task Start()
        {
            Server = new Server(new WvsTradeSocketFactory(this));

            await base.Start();
            await Server.Start(Info.Host, Info.Port);
        }

        public override async Task Stop()
        {
            await base.Stop();
            await Server.Stop();
        }
    }
}