using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Handlers
{
    public class SetGenderHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var cancel = !packet.Decode<bool>();
            var gender = (byte) (packet.Decode<bool>() ? 1 : 0);

            if (adapter.Account == null) return;
            if (adapter.Account.Gender != null) return;

            using var p = new Packet(SendPacketOperations.SetAccountResult);

            adapter.Account.Gender = gender;

            p.Encode<byte>(gender);
            p.Encode<bool>(true);
            await adapter.SendPacket(p);
        }
    }
}