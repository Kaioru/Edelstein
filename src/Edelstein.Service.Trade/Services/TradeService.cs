using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Database.Store;
using Edelstein.Network;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Trade.Services
{
    public class TradeService : AbstractMigrateableService<TradeServiceInfo>
    {
        public IDataStore DataStore { get; }

        public TradeService(
            IOptions<TradeServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDataStore dataStore
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = dataStore;
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new TradeSocket(channel, seqSend, seqRecv, this);
    }
}