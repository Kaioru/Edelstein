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
            var accountID = character.Data.Account.ID.ToString();
            var characterID = character.ID.ToString();

            if (await MigrationCache.ExistsAsync(characterID))
                return false;
            await AccountStatusCache.SetAsync(
                accountID,
                AccountState.MigratingIn,
                15.Seconds()
            );
            await MigrationCache.AddAsync(
                characterID,
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
            var accountID = character.Data.Account.ID.ToString();
            var characterID = character.ID.ToString();

            if (!await MigrationCache.ExistsAsync(characterID))
                return false;
            var migration = await MigrationCache.GetAsync<MigrationInfo>(characterID);
            if (migration.Value.ToService != current.Name)
                return false;
            await AccountStatusCache.SetAsync(accountID, AccountState.LoggedIn);
            await MigrationCache.RemoveAsync(characterID);
            return true;
        }
    }
}