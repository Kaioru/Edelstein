using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckUserLimitHandler : AbstractLoginPacketHandler
{
    public override short Operation => 157;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var packet = new PacketWriter();

        packet.WriteShort(38);

        packet.WriteByte(0);
        packet.WriteByte(0);

        return user.Dispatch(packet);
    }
}
