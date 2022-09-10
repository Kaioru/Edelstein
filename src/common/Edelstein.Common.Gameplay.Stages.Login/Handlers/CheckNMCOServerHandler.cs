using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckNMCOServerHandler : AbstractLoginPacketHandler
{
    public override short Operation => (short)PacketRecvOperations.CheckNMCOServer;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override Task Handle(ILoginStageUser user, IPacketReader reader) =>
        user.Dispatch(
            new PacketWriter(PacketSendOperations.CheckNMCOServerResult)
                .WriteBool(false)
        );
}
