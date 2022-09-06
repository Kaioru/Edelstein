using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class PrivateServerPacketHandler : AbstractLoginPacketHandler
{
    public override short Operation => 134;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var packet = new PacketWriter();

        packet.WriteShort(23);
        packet.WriteInt(23 ^ reader.ReadInt());

        return user.Dispatch(packet);
    }
}
