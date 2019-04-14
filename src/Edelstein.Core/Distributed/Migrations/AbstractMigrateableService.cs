using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed.Migrations
{
    public abstract class AbstractMigrateableService<TInfo> : AbstractPeerServerService<TInfo>
        where TInfo : ServerServiceInfo
    {
        public ICacheClient AccountStateCache { get; }
        public ICacheClient CharacterStateCache { get; }
        public ICacheClient MigrationStateCache { get; }

        protected AbstractMigrateableService(
            TInfo info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory
        ) : base(info, cacheClient, messageBusFactory)
        {
            AccountStateCache = new ScopedCacheClient(cacheClient, Scopes.MigrationAccountCache);
            CharacterStateCache = new ScopedCacheClient(cacheClient, Scopes.MigrationCharacterCache);
            MigrationStateCache = new ScopedCacheClient(cacheClient, Scopes.MigrationCache);
        }
    }
}