using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed.Migrations
{
    public abstract class AbstractMigrateableSocket<TInfo> : AbstractSocket
        where TInfo : ServerServiceInfo
    {
        private readonly AbstractMigrateableService<TInfo> _service;
        private DateTime LastSentHeartbeatDate { get; set; }
        private DateTime LastRecvHeartbeatDate { get; set; }

        public AbstractMigrateableSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            AbstractMigrateableService<TInfo> service
        ) : base(channel, seqSend, seqRecv)
        {
            _service = service;
            LastSentHeartbeatDate = DateTime.Now;
            LastRecvHeartbeatDate = DateTime.Now;
        }

        public async Task TryMigrateTo(Account account, Character character, ServerServiceInfo to)
        {
            if (account == null || character == null)
                throw new MigrationException("account or character is null");
            if (await _service.MigrationStateCache.ExistsAsync(character.ID.ToString()))
                throw new MigrationException("Already migrating");

            account.PreviousConnectedService = _service.Info.Name;
            
            await OnUpdate();
            await _service.AccountStateCache.SetAsync(
                account.ID.ToString(),
                MigrationState.Migrating,
                TimeSpan.FromSeconds(15)
            );
            await _service.MigrationStateCache.SetAsync(
                character.ID.ToString(),
                to.Name,
                TimeSpan.FromSeconds(15)
            );
            await SendPacket(GetMigrationPacket(to));
        }

        public async Task TryMigrateFrom(Account account, Character character)
        {
            if (account == null || character == null)
                throw new MigrationException("Account or Character is null");
            if (!await _service.MigrationStateCache.ExistsAsync(character.ID.ToString()))
                throw new MigrationException("Not migrating");

            var target = (await _service.MigrationStateCache.GetAsync<string>(character.ID.ToString())).Value;

            if (_service.Info.Name != target)
                throw new MigrationException("Migration target is not the current service");

            await _service.MigrationStateCache.RemoveAsync(character.ID.ToString());
            await TryProcessHeartbeat(account, character);
        }

        public async Task TrySendHeartbeat()
        {
            if ((DateTime.Now - LastRecvHeartbeatDate).Minutes >= 1)
            {
                await Close();
                return;
            }

            if ((DateTime.Now - LastSentHeartbeatDate).Seconds >= 30)
            {
                using var p = new Packet(SendPacketOperations.AliveReq);
                LastSentHeartbeatDate = DateTime.Now;
                await SendPacket(p);
            }
        }

        public async Task TryProcessHeartbeat(Account account, Character character, bool initial = false)
        {
            LastRecvHeartbeatDate = DateTime.Now;
            
            if (account == null) return;
            if (!await _service.AccountStateCache.ExistsAsync(account.ID.ToString()) && !initial)
            {
                await Close();
                return;
            }

            await _service.AccountStateCache.SetAsync(
                account.ID.ToString(),
                MigrationState.LoggedIn,
                TimeSpan.FromMinutes(1)
            );

            if (character == null) return;
            await _service.CharacterStateCache.SetAsync(
                character.ID.ToString(),
                _service.Info.Name,
                TimeSpan.FromMinutes(1)
            );
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