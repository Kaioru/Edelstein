using Edelstein.Core.Distributed.States;
using Edelstein.Core.Services.Migrations;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Service.Game.Handlers;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game
{
    public class GameService : AbstractMigrationService<GameServiceState>
    {
        public GameService(
            IOptions<GameServiceState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBusFactory busFactory
        ) : base(state.Value, dataStore, cache, busFactory)
        {
            Handlers[RecvPacketOperations.MigrateIn] = new MigrateInHandler();
        }

        public override ISocketAdapter Build(ISocket socket)
            => new GameServiceAdapter(socket, this);
    }
}