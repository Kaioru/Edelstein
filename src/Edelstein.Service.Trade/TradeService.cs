using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Gameplay.Migrations.States;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Provider;
using Edelstein.Service.Trade.Handlers;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Trade
{
    public class TradeService : AbstractMigrationService<TradeNodeState>
    {
        public IDataTemplateManager TemplateManager { get; }
        public ISocialPartyManager PartyManager { get; }
        public ISocialGuildManager GuildManager { get; }

        public TradeService(
            IOptions<TradeNodeState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBusFactory busFactory,
            ILockProvider lockProvider,
            IDataTemplateManager templateManager
        ) : base(state.Value, dataStore, cache, busFactory)
        {
            TemplateManager = templateManager;
            PartyManager = new SocialPartyManager(
                -1,
                this,
                dataStore,
                lockProvider,
                cache
            );
            GuildManager = new SocialGuildManager(
                this,
                dataStore,
                lockProvider,
                cache
            );

            Handlers[RecvPacketOperations.MigrateIn] = new MigrateInHandler();
            Handlers[RecvPacketOperations.UserTransferFieldRequest] = new UserTransferFieldRequest();
        }

        public override ISocketAdapter Build(ISocket socket)
            => new TradeServiceAdapter(socket, this);
    }
}