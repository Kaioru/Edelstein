using Edelstein.Core.Services.Distributed.States;
using Edelstein.Core.Services.Migrations;
using Edelstein.Database;
using Edelstein.Network;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login.Service
{
    public class LoginService : AbstractMigrationService<LoginServiceState>
    {
        public LoginService(
            IOptions<LoginServiceState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBus bus
        ) : base(state.Value, dataStore, cache, bus)
        {
        }

        public override ISocketAdapter Build(ISocket socket)
            => new LoginServiceAdapter(socket, this);
    }
}