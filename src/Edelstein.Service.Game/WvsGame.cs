using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Service.Game.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Service.Game
{
    public class WvsGame : AbstractService<GameServiceInfo>
    {
        private IServer Server { get; set; }

        public WvsGame(
            GameServiceInfo info,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory
        ) : base(info, cache, messageBus, dataContextFactory)
        {
        }
        
        public WvsGame(
            WvsGameOptions options,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory
        ) : base(options.Service, cache, messageBus, dataContextFactory)
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