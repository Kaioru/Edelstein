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

        public async Task<bool> TryMigrateTo(int accountID, int characterID, ServiceInfo to)
        {
            if (await MigrationCache.ExistsAsync(characterID.ToString()))
                return false;
            await AccountStatusCache.SetAsync(
                accountID.ToString(),
                AccountState.MigratingIn,
                15.Seconds()
            );
            await MigrationCache.AddAsync(
                characterID.ToString(),
                new MigrationInfo
                {
                    ID = characterID,
                    FromService = Info.Name,
                    ToService = to.Name
                },
                15.Seconds()
            );
            return true;
        }

        public async Task<bool> TryMigrateFrom(int accountID, int characterID)
        {
            if (!await MigrationCache.ExistsAsync(characterID.ToString()))
                return false;
            await AccountStatusCache.SetAsync(accountID.ToString(), AccountState.LoggedIn);
            await MigrationCache.RemoveAsync(characterID.ToString());
            return true;
        }
    }
}