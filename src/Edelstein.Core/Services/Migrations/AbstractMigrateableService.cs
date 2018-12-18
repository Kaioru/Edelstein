using System;
using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Context;
using Edelstein.Data.Entities;
using Foundatio.Caching;
using Foundatio.Messaging;
using Humanizer;

namespace Edelstein.Core.Services.Migrations
{
    public class AbstractMigrateableService<TInfo> : AbstractService<TInfo>, IMigrateable
        where TInfo : ServiceInfo
    {
        public AbstractMigrateableService(
            TInfo info,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory
        ) : base(info, cache, messageBus, dataContextFactory)
        {
        }

        public async Task<bool> TryMigrateTo(int id, ServiceInfo to)
        {
            if (await MigrationCache.ExistsAsync(id.ToString()))
                return false;
            await AccountStatusCache.SetAsync(
                id.ToString(),
                AccountState.MigratingIn,
                15.Seconds()
            );
            await MigrationCache.AddAsync(
                id.ToString(),
                new MigrationInfo
                {
                    ID = id,
                    FromService = Info.Name,
                    ToService = to.Name
                },
                15.Seconds()
            );
            return true;
        }

        public async Task<bool> TryMigrateFrom(int id)
        {
            if (!await MigrationCache.ExistsAsync(id.ToString()))
                return false;
            await AccountStatusCache.SetAsync(id.ToString(), AccountState.LoggedIn);
            await MigrationCache.RemoveAsync(id.ToString());
            return true;
        }
    }
}