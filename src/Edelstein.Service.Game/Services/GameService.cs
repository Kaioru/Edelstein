using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Network;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game.Services
{
    public class GameService : AbstractMigrateableService<GameServiceInfo>
    {
        public GameService(
            IOptions<GameServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
        {
            throw new System.NotImplementedException();
        }
    }
}