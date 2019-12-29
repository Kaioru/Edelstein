using Edelstein.Core.Distributed.States;
using Edelstein.Core.Services.Migrations;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game.Services
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
        }

        public override ISocketAdapter Build(ISocket socket)
            => new GameServiceAdapter(socket, this);
    }
}