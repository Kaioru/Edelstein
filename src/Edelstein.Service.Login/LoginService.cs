using Edelstein.Core.Distributed.States;
using Edelstein.Core.Services.Migrations;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Provider;
using Edelstein.Service.Login.Handlers;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login
{
    public class LoginService : AbstractMigrationService<LoginServiceState>
    {
        public const string AuthLockKey = "lock:auth";
        public const string CreateCharLockKey = "lock:createChar";
        public ILockProvider LockProvider { get; }
        public IDataTemplateManager TemplateManager { get; }

        public LoginService(
            IOptions<LoginServiceState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBusFactory busFactory,
            ILockProvider lockProvider,
            IDataTemplateManager templateManager
        ) : base(state.Value, dataStore, cache, busFactory)
        {
            LockProvider = lockProvider;
            TemplateManager = templateManager;
            
            Handlers[RecvPacketOperations.CheckPassword] = new CheckPasswordHandler();
            Handlers[RecvPacketOperations.WorldInfoRequest] = new WorldRequestHandler();
            Handlers[RecvPacketOperations.SelectWorld] = new SelectWorldHandler();
            Handlers[RecvPacketOperations.CheckUserLimit] = new CheckUserLimitHandler();
            Handlers[RecvPacketOperations.SetGender] = new SetGenderHandler();
            Handlers[RecvPacketOperations.WorldRequest] = new WorldRequestHandler();
            Handlers[RecvPacketOperations.CheckDuplicatedID] = new CheckDuplicatedIDHandler();
        }

        public override ISocketAdapter Build(ISocket socket)
            => new LoginServiceAdapter(socket, this);
    }
}