using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckPasswordHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ICheckPassword> _pipeline;

    public CheckPasswordHandler(IPipeline<ICheckPassword> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.CheckPassword;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new CheckPassword(
            user,
            reader.ReadString(),
            reader.ReadString()
        );

        return _pipeline.Process(message);
    }
}
