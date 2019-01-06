using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Migrations;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Trade.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Trade
{
    public class WvsTrade : AbstractMigrateableService<TradeServiceInfo>
    {
        private IServer Server { get; set; }
        public ITemplateManager TemplateManager { get; }

        public WvsTrade(
            IApplicationLifetime appLifetime,
            ICacheClient cache,
            IMessageBus messageBus,
            IOptions<TradeServiceInfo> info,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : base(appLifetime, cache, messageBus, info, dataContextFactory)
        {
            TemplateManager = templateManager;
        }

        protected override async Task OnStarted()
        {
            Server = new Server(new WvsTradeSocketFactory(this));

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