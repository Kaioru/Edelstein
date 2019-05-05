using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Shop.Extensions;
using Edelstein.Service.Shop.Logging;
using Edelstein.Service.Shop.Types;
using Foundatio.Caching;
using Marten.Util;
using MoreLinq.Extensions;

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

        public override async Task OnUpdate()
        {
            using (var store = Service.DataStore.OpenSession())
            {
                await store.UpdateAsync(Account);
                await store.UpdateAsync(AccountData);
                await store.UpdateAsync(Character);
            }
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

        public async Task SendLockerData()
        {
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            using (var store = Service.DataStore.OpenSession())
            {
                p.Encode<byte>((byte) CashItemResult.LoadLocker_Done);

                var locker = AccountData.Locker;

                p.Encode<short>((short) locker.Items.Count);
                locker.Items.ForEach(i => i.Encode(p));

                p.Encode<short>(AccountData.Trunk.SlotMax);
                p.Encode<short>((short) AccountData.SlotCount);
                p.Encode<short>(0);
                p.Encode<short>((short) store
                    .Query<Character>()
                    .Count(c => c.AccountDataID == AccountData.ID));
                await SendPacket(p);
            }
        }

        public async Task SendWishListData()
        {
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.LoadWish_Done);
                Character.WishList.ForEach(w => p.Encode<int>(w));
                await SendPacket(p);
            }
        }
        
        public async Task SendCashData()
        {
            using (var p = new Packet(SendPacketOperations.CashShopQueryCashResult))
            {
                p.Encode<int>(Account.NexonCash);
                p.Encode<int>(Account.MaplePoint);
                p.Encode<int>(Account.PrepaidNXCash);
                await SendPacket(p);
            }
        }
    }
}