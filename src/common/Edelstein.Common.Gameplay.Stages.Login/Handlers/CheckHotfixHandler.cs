using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckHotfixHandler : AbstractLoginPacketHandler
{
    public override short Operation => 152;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var packet = new PacketWriter();

        packet.WriteShort(36);
        packet.WriteBool(true);

        return user.Dispatch(packet);
    }
}
