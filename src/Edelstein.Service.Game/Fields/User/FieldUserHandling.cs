using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
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
            else if (!await Socket.WvsGame.TryMigrateTo(Character, service)) result = 0x1;

            if (result == 0x0)
            {
                using (var p = new Packet(SendPacketOperations.MigrateCommand))
                {
                    p.Encode<bool>(true);

                    var endpoint = new IPEndPoint(IPAddress.Parse(service.Host), service.Port);
                    var address = endpoint.Address.MapToIPv4().GetAddressBytes();
                    var port = endpoint.Port;

                    address.ForEach(b => p.Encode<byte>(b));
                    p.Encode<short>((short) port);
                    await SendPacket(p);
                }

                return;
            }

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