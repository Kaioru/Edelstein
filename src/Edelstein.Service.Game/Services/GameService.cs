using Edelstein.Core.Services.Distributed.States;
using Edelstein.Core.Services.Migrations;
using Edelstein.Database;
using Edelstein.Network;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game.Services
{
    public class GameService : AbstractMigrationService<GameServiceState>
    {
        public GameService(
            IOptions<GameServiceState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBus bus
        ) : base(state.Value, dataStore, cache, bus)
        {
        }

        public override ISocketAdapter Build(ISocket socket)
            => new GameServiceAdapter(socket, this);
    }
}