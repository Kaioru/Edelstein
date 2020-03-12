using System.Net;
using Edelstein.Core.Distributed;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login
{
    public class LoginServiceAdapter : AbstractMigrationSocketAdapter
    {
        public LoginService Service { get; }
        public IServerNodeState SelectedNode { get; set; }

        public LoginServiceAdapter(
            ISocket socket,
            LoginService service
        ) : base(socket, service)
        {
            Service = service;
        }

        public override IPacket GetMigrationPacket(IServerNodeState to)
        {
            using var p = new Packet(SendPacketOperations.SelectCharacterResult);

            p.Encode<byte>(0);
            p.Encode<byte>(0);

            var endpoint = new IPEndPoint(IPAddress.Parse(to.Host), to.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = endpoint.Port;

            foreach (var b in address)
                p.Encode<byte>(b);
            p.Encode<short>((short) port);

            p.Encode<int>(Character.ID);
            p.Encode<byte>(0);
            p.Encode<int>(0);

            return p;
        }
    }
}