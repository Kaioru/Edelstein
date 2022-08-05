using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckUserLimitHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ICheckUserLimit> _pipeline;

    public CheckUserLimitHandler(IPipeline<ICheckUserLimit> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.CheckUserLimit;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new CheckUserLimit(
            user,
            reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
