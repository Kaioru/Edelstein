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

        public async Task<bool> TryMigrateTo(Character character, ServiceInfo to)
        {
            if (await MigrationCache.ExistsAsync(character.ID.ToString()))
                return false;
            await AccountStatusCache.SetAsync(
                character.Data.Account.ID.ToString(),
                AccountState.MigratingIn,
                15.Seconds()
            );
            await MigrationCache.AddAsync(
                character.ID.ToString(),
                new MigrationInfo
                {
                    ID = character.ID,
                    FromService = Info.Name,
                    ToService = to.Name
                },
                15.Seconds()
            );
            return true;
        }

        public async Task<bool> TryMigrateFrom(Character character, ServiceInfo current)
        {
            if (!await MigrationCache.ExistsAsync(character.ID.ToString()))
                return false;
            var migration = await MigrationCache.GetAsync<MigrationInfo>(character.ID.ToString());
            if (migration.Value.ToService != current.Name)
                return false;
            await AccountStatusCache.SetAsync(character.Data.Account.ID.ToString(), AccountState.LoggedIn);
            await MigrationCache.RemoveAsync(character.ID.ToString());
            return true;
        }
    }
}