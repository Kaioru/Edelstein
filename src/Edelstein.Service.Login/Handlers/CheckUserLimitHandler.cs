using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Handlers
{
    public class CheckUserLimitHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            if (adapter.Account == null) return;

            using var p = new Packet(SendPacketOperations.CheckUserLimitResult);

            p.Encode<byte>(0); // TODO: bOverUserLimit
            p.Encode<byte>(0); // TODO: bPopulateLevel

            await adapter.SendPacket(p);
        }
    }
}