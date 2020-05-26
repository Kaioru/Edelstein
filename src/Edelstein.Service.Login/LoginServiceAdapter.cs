using System.Net;
using Edelstein.Core.Distributed;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Network;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;

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
            using var p = new OutPacket(SendPacketOperations.SelectCharacterResult);

            p.EncodeByte(0);
            p.EncodeByte(0);

            var endpoint = new IPEndPoint(IPAddress.Parse(to.Host), to.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = endpoint.Port;

            foreach (var b in address)
                p.EncodeByte(b);
            p.EncodeShort((short) port);

            p.EncodeInt(Character.ID);
            p.EncodeByte(0);
            p.EncodeInt(0);

            return p;
        }
    }
}