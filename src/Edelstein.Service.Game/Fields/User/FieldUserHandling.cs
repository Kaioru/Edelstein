using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        private async Task OnUserTransferChannelRequest(IPacket packet)
        {
            // TODO: temporary; not implemented fully
            var channel = packet.Decode<byte>();
            var service = Socket.WvsGame.Peers
                .OfType<GameServiceInfo>()
                .ToList()[channel];

            if (service != null)
            {
                if (await Socket.WvsGame.TryMigrateTo(Character.Data.Account.ID, service))
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
                }
            }
        }
    }
}