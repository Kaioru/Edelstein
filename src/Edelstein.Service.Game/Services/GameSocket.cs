using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Game.Services
{
    public partial class GameSocket : AbstractMigrateableSocket<GameServiceInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public GameService Service { get; }

        public Account Account { get; set; }
        public AccountData AccountData { get; set; }
        public Character Character { get; set; }
        public FieldUser FieldUser { get; set; }

        public GameSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            GameService service
        ) : base(channel, seqSend, seqRecv, service)
        {
            Service = service;
        }

        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();
            return operation switch {
                RecvPacketOperations.MigrateIn => OnMigrateIn(packet),
                RecvPacketOperations.AliveAck => TryProcessHeartbeat(Account, Character),
                _ => Task.Run(() => Logger.Warn($"Unhandled packet operation {operation}"))
                };
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Socket caught exception");
            return Task.CompletedTask;
        }

        public override async Task OnUpdate()
        {
            using (var store = Service.DocumentStore.OpenSession())
            {
                store.Update(Account);
                store.Update(AccountData);
                store.Update(Character);
                await store.SaveChangesAsync();
            }
        }

        public override async Task OnDisconnect()
        {
            if (FieldUser != null) await FieldUser.Field.Leave(FieldUser);
            if (Account == null) return;

            var state = (await Service.AccountStateCache.GetAsync<MigrationState>(Account.ID.ToString())).Value;

            if (state != MigrationState.Migrating)
            {
                await OnUpdate();
                await Service.AccountStateCache.RemoveAsync(Account.ID.ToString());
            }
        }
    }
}