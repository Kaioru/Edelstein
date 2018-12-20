using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field;
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
    }
}