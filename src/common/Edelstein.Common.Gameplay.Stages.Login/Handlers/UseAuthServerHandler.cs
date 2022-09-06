using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class UseAuthServerHandler : AbstractLoginPacketHandler
{
    public override short Operation => 0xAB;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var packet = new PacketWriter();

        packet.WriteShort(47);
        packet.WriteBool(false);

        return user.Dispatch(packet);
    }
}
