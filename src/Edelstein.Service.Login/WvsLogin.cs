using System.Net;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Migrations;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates;
using Edelstein.Service.Login.Sockets;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;

namespace Edelstein.Service.Login
{
    public class WvsLogin : AbstractMigrateableService<LoginServiceInfo>
    {
        private IServer Server { get; set; }
        public ILockProvider LockProvider { get; }
        public ITemplateManager TemplateManager { get; }

        public WvsLogin(
            LoginServiceInfo info,
            ICacheClient cache,
            IMessageBus messageBus,
            ILockProvider lockProvider,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : base(info, cache, messageBus, dataContextFactory)
        {
            LockProvider = lockProvider;
            TemplateManager = templateManager;
        }

        public WvsLogin(
            WvsLoginOptions options,
            ICacheClient cache,
            IMessageBus messageBus,
            ILockProvider lockProvider,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager
        ) : this(options.Service, cache, messageBus, lockProvider, dataContextFactory, templateManager)
        {
        }

        public override async Task Start()
        {
            Server = new Server(new WvsLoginSocketFactory(this));

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