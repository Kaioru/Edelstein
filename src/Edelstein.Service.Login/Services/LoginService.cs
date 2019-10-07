using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database.Store;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Login.Services
{
    public class LoginService : AbstractMigrateableService<LoginServiceInfo>
    {
        public IDataStore DataStore { get; }
        public ILockProvider LockProvider { get; }
        public ITemplateManager TemplateManager { get; }

        public LoginService(
            IOptions<LoginServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDataStore store,
            ILockProvider lockProvider,
            ITemplateManager templateManager
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = store;
            LockProvider = lockProvider;
            TemplateManager = templateManager;
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new LoginSocket(channel, seqSend, seqRecv, this);
    }
}