using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Migrations;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Fields;
using Edelstein.Service.Game.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Service.Game
{
    public class WvsGame : AbstractMigrateableService<GameServiceInfo>
    {
        private IServer Server { get; set; }
        public ITemplateManager TemplateManager { get; }

        public FieldManager FieldManager { get; }

        public WvsGame(
            GameServiceInfo info,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : base(info, cache, messageBus, dataContextFactory)
        {
            TemplateManager = templateManager;
            FieldManager = new FieldManager(TemplateManager);
        }

        public WvsGame(
            WvsGameOptions options,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : this(options.Service, cache, messageBus, dataContextFactory, templateManager)
        {
        }

        public override async Task Start()
        {
            Server = new Server(new WvsGameSocketFactory(this));
            
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