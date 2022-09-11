using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class LogoutWorldHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<IWorldSelectReset> _pipeline;

    public LogoutWorldHandler(IPipeline<IWorldSelectReset> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.LogoutWorld;

    public override Task Handle(ILoginStageUser user, IPacketReader reader) =>
        _pipeline.Process(new WorldSelectReset(user));
}
