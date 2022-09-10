using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class WorldInfoRequestHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<IWorldList> _pipeline;

    public WorldInfoRequestHandler(IPipeline<IWorldList> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.WorldInfoRequest;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckToken;

    public override Task Handle(ILoginStageUser user, IPacketReader reader) =>
        _pipeline.Process(new WorldList(user));
}
