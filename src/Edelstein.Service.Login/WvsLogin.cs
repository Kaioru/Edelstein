using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network;
using Edelstein.Service.Login.Logging;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Service.Login
{
    public class WvsLogin : AbstractService<LoginServiceInfo>
    {
        private IServer Server { get; set; }

        public WvsLogin(
            WvsLoginOptions options,
            ICacheClient cache,
            IMessageBus messageBus
        ) : base(options.Service, cache, messageBus)
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