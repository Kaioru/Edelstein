using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Migrations;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Login.Sockets;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login
{
    public class WvsLogin : AbstractMigrateableService<LoginServiceInfo>
    {
        private IServer Server { get; set; }
        public ILockProvider LockProvider { get; }
        public ITemplateManager TemplateManager { get; }

        public WvsLogin(
            IApplicationLifetime appLifetime,
            ICacheClient cache,
            IMessageBus messageBus,
            IOptions<LoginServiceInfo> info,
            IDataContextFactory dataContextFactory,
            ILockProvider lockProvider,
            ITemplateManager templateManager
        ) : base(appLifetime, cache, messageBus, info, dataContextFactory)
        {
            LockProvider = lockProvider;
            TemplateManager = templateManager;
        }

        protected override async Task OnStarted()
        {
            Server = new Server(new WvsLoginSocketFactory(this));

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