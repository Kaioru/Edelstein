using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class SelectWorldHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<IWorldSelect> _pipeline;

    public SelectWorldHandler(IPipeline<IWorldSelect> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.SelectWorld;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadByte();
        var worldID = reader.ReadByte();
        var channelID = reader.ReadByte();

        return _pipeline.Process(new WorldSelect(user, worldID, channelID));
    }
}
