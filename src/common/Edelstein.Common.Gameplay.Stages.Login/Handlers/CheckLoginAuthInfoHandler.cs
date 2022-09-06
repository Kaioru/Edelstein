using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckLoginAuthInfoHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ICheckLoginAuthInfo> _pipeline;

    public CheckLoginAuthInfoHandler(IPipeline<ICheckLoginAuthInfo> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.CheckLoginAuthInfo;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadByte();
        var password = reader.ReadString();
        var username = reader.ReadString();

        return _pipeline.Process(new CheckLoginAuthInfo(user, username, password));
    }
}
