using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class LoginBasicInfoHandler : AbstractLoginPacketHandler
{
    public override short Operation => (short)PacketRecvOperations.LoginBasicInfo;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        if (reader.ReadBool())
            user.State = LoginState.CheckToken;
        return Task.CompletedTask;
    }
}
