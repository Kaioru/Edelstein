using Edelstein.Core.Distributed.States;
using Edelstein.Core.Services.Migrations;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login.Services
{
    public class LoginService : AbstractMigrationService<LoginServiceState>
    {
        public LoginService(
            IOptions<LoginServiceState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBusFactory busFactory
        ) : base(state.Value, dataStore, cache, busFactory)
        {
        }

        public override ISocketAdapter Build(ISocket socket)
            => new LoginServiceAdapter(socket, this);
    }
}