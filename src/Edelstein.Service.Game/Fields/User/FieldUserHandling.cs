using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        private async Task OnUserTransferFieldRequest(IPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();

            var name = packet.Decode<string>();
            var portal = Field.GetPortal(name);

            await portal.Enter(this);
        }

        private async Task OnUserMove(IPacket packet)
        {
            packet.Decode<long>();
            packet.Decode<byte>();
            packet.Decode<long>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();

            var path = Move(packet);

            using (var p = new Packet(SendPacketOperations.UserMove))
            {
                p.Encode<int>(ID);
                path.Encode(p);
                await Field.BroadcastPacket(this, p);
            }
        }

        private async Task OnUserChat(IPacket packet)
        {
            packet.Decode<int>();

            var message = packet.Decode<string>();
            var onlyBalloon = packet.Decode<bool>();

            using (var p = new Packet(SendPacketOperations.UserChat))
            {
                p.Encode<int>(ID);
                p.Encode<bool>(false);
                p.Encode<string>(message);
                p.Encode<bool>(onlyBalloon);
                await Field.BroadcastPacket(p);
            }
        }

        private async Task OnUserEmotion(IPacket packet)
        {
            var emotion = packet.Decode<int>();
            var duration = packet.Decode<int>();
            var byItemOption = packet.Decode<bool>();

            // TODO: item option checks

            using (var p = new Packet(SendPacketOperations.UserEmotion))
            {
                p.Encode<int>(ID);
                p.Encode<int>(emotion);
                p.Encode<int>(duration);
                p.Encode<bool>(byItemOption);
                await Field.BroadcastPacket(this, p);
            }
        }
    }
}