using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Core.Logging;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Context;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Network.Packet;
using Foundatio.Caching;
using Foundatio.Messaging;
using Humanizer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MoreLinq;

namespace Edelstein.Core.Services.Migrations
{
    public class AbstractMigrateableService<TInfo> : AbstractHostedService<TInfo>, IMigrateable
        where TInfo : ServiceInfo, new()
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public AbstractMigrateableService(
            IApplicationLifetime appLifetime,
            ICacheClient cache,
            IMessageBus messageBus,
            IOptions<TInfo> info,
            IDataContextFactory dataContextFactory
        ) : base(appLifetime, cache, messageBus, info, dataContextFactory)
        {
        }

        protected override async Task OnStarted()
        {
            using (var db = DataContextFactory.Create())
            {
                var accounts = db.Accounts
                    .Where(a => a.LatestConnectedService == Info.Name)
                    .Select(a => a.ID.ToString());

                if (accounts.Any())
                {
                    await AccountStatusCache.RemoveAllAsync(db.Accounts
                        .Where(a => a.LatestConnectedService == Info.Name)
                        .Select(a => a.ID.ToString()));
                    Logger.Info(
                        $"Forcibly reset account states of {"account".ToQuantity(accounts.Count())} previously connected to {Info.Name}"
                    );
                }
            }

            await base.OnStarted();
        }

        public async Task<bool> TryMigrateTo(
            ISocket socket, Character character, ServerServiceInfo to,
            Func<ServerServiceInfo, IPacket> getMigrationCommand = null)
        {
            var accountID = character.Data.Account.ID.ToString();
            var characterID = character.ID.ToString();

            if (await MigrationCache.ExistsAsync(characterID))
                return false;

            socket.ReadOnlyMode = true;

            await socket.OnUpdate();
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

            if (getMigrationCommand == null)
                getMigrationCommand = info =>
                {
                    using (var p = new Packet(SendPacketOperations.MigrateCommand))
                    {
                        p.Encode<bool>(true);

                        var endpoint = new IPEndPoint(IPAddress.Parse(info.Host), info.Port);
                        var address = endpoint.Address.MapToIPv4().GetAddressBytes();
                        var port = endpoint.Port;

                        address.ForEach(b => p.Encode<byte>(b));
                        p.Encode<short>((short) port);
                        return p;
                    }
                };
            character.Data.Account.LatestConnectedService = to.Name;
            character.Data.Account.PreviousConnectedService = Info.Name;
            await socket.SendPacket(getMigrationCommand.Invoke(to));
            return true;
        }

        public async Task<bool> TryMigrateFrom(Character character, ServerServiceInfo current)
        {
            var accountID = character.Data.Account.ID.ToString();
            var characterID = character.ID.ToString();

            if (!await MigrationCache.ExistsAsync(characterID))
                return false;
            var migration = (await MigrationCache.GetAsync<MigrationInfo>(characterID)).Value;
            if (migration.ToService != current.Name)
                return false;
            character.Data.Account.LatestConnectedService = migration.ToService;
            character.Data.Account.PreviousConnectedService = migration.FromService;
            await AccountStatusCache.SetAsync(accountID, AccountState.LoggedIn);
            await MigrationCache.RemoveAsync(characterID);
            return true;
        }
    }
}