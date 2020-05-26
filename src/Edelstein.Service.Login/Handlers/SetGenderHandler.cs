using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;

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