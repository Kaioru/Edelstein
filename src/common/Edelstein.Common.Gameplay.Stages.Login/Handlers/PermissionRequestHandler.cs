using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class PermissionRequestHandler : AbstractLoginPacketHandler
{
    public override short Operation => 103;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override Task Handle(ILoginStageUser user, IPacketReader reader) => Task.CompletedTask;
}
