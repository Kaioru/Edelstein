using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckUserLimitHandler : AbstractLoginPacketHandler
{
    public override short Operation => (short)PacketRecvOperations.CheckUserLimit;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var packet = new PacketWriter(PacketSendOperations.CheckUserLimitResult);

        packet.WriteByte(0);
        packet.WriteByte(0);

        return user.Dispatch(packet);
    }
}
