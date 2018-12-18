using System;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Context;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Foundatio.Caching;
using Foundatio.Messaging;
using Humanizer;
using MoreLinq;

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

        public async Task<bool> TryMigrateTo(
            ISocket socket, Character character, ServerServiceInfo to,
            Func<ServerServiceInfo, IPacket> getMigrationCommand = null)
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