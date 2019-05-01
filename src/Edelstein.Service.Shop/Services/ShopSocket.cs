using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Shop.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Shop.Services
{
    public partial class ShopSocket : AbstractMigrateableSocket<ShopServiceInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public ShopService Service { get; }

        public Account Account { get; set; }
        public AccountData AccountData { get; set; }
        public Character Character { get; set; }

        public ShopSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            ShopService service
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
                RecvPacketOperations.UserTransferFieldRequest => OnUserTransferFieldRequest(packet),
                _ => Task.Run(() => Logger.Warn($"Unhandled packet operation {operation}"))
                };
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Socket caught exception");
            return Task.CompletedTask;
        }

        public override Task OnUpdate()
        {
            return Task.CompletedTask;
        }

        public override async Task OnDisconnect()
        {
            if (Account == null) return;
            var state = (await Service.AccountStateCache.GetAsync<MigrationState>(Account.ID.ToString())).Value;
            if (state != MigrationState.Migrating)
            {
                await Service.AccountStateCache.RemoveAsync(Account.ID.ToString());
            }
        }
    }
}