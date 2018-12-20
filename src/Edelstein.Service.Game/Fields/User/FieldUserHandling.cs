using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field;
using Edelstein.Service.Game.Fields.Objects.Drop;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        public Task OnPacket(RecvPacketOperations operation, IPacket packet)
        {
            switch (operation)
            {
                case RecvPacketOperations.UserTransferChannelRequest:
                    return OnUserTransferChannelRequest(packet);
                case RecvPacketOperations.UserMigrateToCashShopRequest:
                    return OnUserMigrateToCashShopRequest(packet);
                case RecvPacketOperations.UserMove:
                    return OnUserMove(packet);
                case RecvPacketOperations.UserGatherItemRequest:
                    return OnUserGatherItemRequest(packet);
                case RecvPacketOperations.UserSortItemRequest:
                    return OnUserSortItemRequest(packet);
                case RecvPacketOperations.UserChangeSlotPositionRequest:
                    return OnUserChangeSlotPositionRequest(packet);
                case RecvPacketOperations.UserDropMoneyRequest:
                    return OnUserDropMoneyRequest(packet);
                case RecvPacketOperations.DropPickUpRequest:
                    return OnDropPickUpRequest(packet);
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    return Task.CompletedTask;
            }
        }

        private async Task OnUserTransferChannelRequest(IPacket packet)
        {
            byte result = 0x0;
            var channel = packet.Decode<byte>();
            var service = Socket.WvsGame.Peers
                .OfType<GameServiceInfo>()
                .Where(g => g.WorldID == Socket.WvsGame.Info.WorldID)
                .OrderBy(g => g.ID)
                .ToList()[channel];

            if (Field.Template.Limit.HasFlag(FieldOpt.MigrateLimit)) return;

            if (service == null) result = 0x1;
            else if (service.AdultChannel) result = 0x1;
            else if (!await Socket.WvsGame.TryMigrateTo(Socket, Character, service)) result = 0x1;

            if (result == 0x0) return;
            using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
            {
                p.Encode<byte>(result);
                await SendPacket(p);
            }
        }

        private async Task OnUserMigrateToCashShopRequest(IPacket packet)
        {
            byte result = 0x0;
            var service = Socket.WvsGame.Peers
                .OfType<ShopServiceInfo>()
                .Where(g => g.Worlds.Contains(Socket.WvsGame.Info.WorldID))
                .OrderBy(g => g.ID)
                .FirstOrDefault();
            // TODO: selection prompt when multiple?

            if (Field.Template.Limit.HasFlag(FieldOpt.MigrateLimit)) return;

            if (service == null) result = 0x2;
            else if (!await Socket.WvsGame.TryMigrateTo(Socket, Character, service)) result = 0x2;

            if (result == 0x0) return;
            using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
            {
                p.Encode<byte>(result);
                await SendPacket(p);
            }
        }

        private Task OnUserMove(IPacket packet)
        {
            packet.Decode<long>();
            packet.Decode<byte>();
            packet.Decode<long>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();

            return Move(packet);
        }

        private async Task OnUserGatherItemRequest(IPacket packet)
        {
            packet.Decode<int>();
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventoryCopy = Character.GetInventory(inventoryType).Items
                .Where(i => i.Position > 0)
                .OrderBy(i => i.Position)
                .ToList();
            short pos = 1;

            await ModifyInventory(i =>
            {
                inventoryCopy.ForEach(s => i.Remove(s));
                inventoryCopy.ForEach(item => item.Position = pos++);
                inventoryCopy.ForEach(i.Set);
            }, true);

            using (var p = new Packet(SendPacketOperations.GatherItemResult))
            {
                p.Encode<bool>(false);
                p.Encode<byte>((byte) inventoryType);
                await SendPacket(p);
            }
        }

        private async Task OnUserSortItemRequest(IPacket packet)
        {
            packet.Decode<int>();
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventoryCopy = Character.GetInventory(inventoryType).Items
                .Where(i => i.Position > 0)
                .OrderBy(i => i.Position)
                .ToList();

            await ModifyInventory(i =>
            {
                inventoryCopy.ForEach(s => i.Remove(s));
                inventoryCopy = inventoryCopy.OrderBy(item => item.TemplateID).ToList();
                inventoryCopy.ForEach(i.Add);
            }, true);

            using (var p = new Packet(SendPacketOperations.SortItemResult))
            {
                p.Encode<bool>(false);
                p.Encode<byte>((byte) inventoryType);
                await SendPacket(p);
            }
        }

        private async Task OnUserChangeSlotPositionRequest(IPacket packet)
        {
            packet.Decode<int>();
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var fromSlot = packet.Decode<short>();
            var toSlot = packet.Decode<short>();

            packet.Decode<short>();

            if (toSlot == 0)
            {
                if (Field.Template.Limit.HasFlag(FieldOpt.DropLimit)) return;
                
                await ModifyInventory(i =>
                {
                    var item = Character.GetInventory(inventoryType).Items
                        .Single(ii => ii.Position == fromSlot);
                    var drop = new FieldDropItem(item) {Position = Position};

                    i.Remove(item);
                    Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
                }, true);
                return;
            }

            await ModifyInventory(i => i.Move(inventoryType, fromSlot, toSlot), true);
        }

        private async Task OnUserDropMoneyRequest(IPacket packet)
        {
            packet.Decode<int>();
            var money = packet.Decode<int>();
            
            if (Field.Template.Limit.HasFlag(FieldOpt.DropLimit)) return;

            await ModifyStats(s =>
            {
                if (s.Money < money) return;
                var drop = new FieldDropMoney(money) {Position = Position};

                s.Money -= money;
                Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
            }, true);
        }

        private Task OnDropPickUpRequest(IPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();
            packet.Decode<short>();
            packet.Decode<short>();
            var objectID = packet.Decode<int>();
            packet.Decode<int>();
            var drop = Field.GetObject<AbstractFieldDrop>(objectID);

            return drop?.PickUp(this);
        }
    }
}