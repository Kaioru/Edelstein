using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed.Migrations
{
    public abstract class AbstractMigrateableSocket<TInfo> : AbstractSocket
        where TInfo : ServerServiceInfo
    {
        private readonly AbstractMigrateableService<TInfo> _service;

        public Account? Account { get; set; }
        public Character? Character { get; set; }

        public AbstractMigrateableSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            AbstractMigrateableService<TInfo> service
        ) : base(channel, seqSend, seqRecv)
        {
            _service = service;
        }

        public async Task TryMigrateTo(ServerServiceInfo to)
        {
            if (Account == null || Character == null)
                throw new MigrationException("account or character is null");
            if (await _service.MigrationStateCache.ExistsAsync(Character.ID.ToString()))
                throw new MigrationException("Already migrating");

            await _service.AccountStateCache.SetAsync(
                Account.ID.ToString(),
                MigrationState.Migrating,
                TimeSpan.FromSeconds(15)
            );
            await _service.MigrationStateCache.SetAsync(
                Character.ID.ToString(),
                to.Name,
                TimeSpan.FromSeconds(15)
            );
            await SendPacket(GetMigrationPacket(to));
        }

        public async Task TryMigrateFrom(ServerServiceInfo current)
        {
            if (Account == null || Character == null)
                throw new MigrationException("account or character is null");
            if (!await _service.MigrationStateCache.ExistsAsync(Character.ID.ToString()))
                throw new MigrationException("Not migrating");

            var target = (await _service.MigrationStateCache.GetAsync<string>(Character.ID.ToString())).Value;

            if (current.Name != target)
                throw new MigrationException("Migration target is not the current service");

            await _service.AccountStateCache.SetAsync(
                Account.ID.ToString(),
                MigrationState.LoggedIn
            );
            await _service.CharacterStateCache.SetAsync(
                Character.ID.ToString(),
                current.Name
            );
            await _service.MigrationStateCache.RemoveAsync(Character.ID.ToString());
        }

        protected virtual IPacket GetMigrationPacket(ServerServiceInfo to)
        {
            using (var p = new Packet(SendPacketOperations.MigrateCommand))
            {
                p.Encode<bool>(true);

                var endpoint = new IPEndPoint(IPAddress.Parse(to.Host), to.Port);
                var address = endpoint.Address.MapToIPv4().GetAddressBytes();
                var port = endpoint.Port;

                foreach (var b in address)
                    p.Encode<byte>(b);
                p.Encode<short>((short) port);
                return p;
            }
        }
    }
}