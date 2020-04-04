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
            IPacketDecoder packet
        )
        {
            var cancel = !packet.DecodeBool();
            var gender = (byte) (packet.DecodeBool() ? 1 : 0);

            if (adapter.Account == null) return;
            if (adapter.Account.Gender != null) return;

            using var p = new OutPacket(SendPacketOperations.SetAccountResult);

            adapter.Account.Gender = gender;

            p.EncodeByte(gender);
            p.EncodeBool(true);
            await adapter.SendPacket(p);
        }
    }
}