using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class EnableSPWRequestHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ISPWCreate> _pipeline;

    public EnableSPWRequestHandler(
        IPipeline<ISPWCreate> pipeline
    ) =>
        _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.EnableSPWRequest;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW == null;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadShort();
        _ = reader.ReadInt();
        _ = reader.ReadString();
        _ = reader.ReadString();
        var spw = reader.ReadString();

        return _pipeline.Process(new SPWCreate(user, spw));
    }
}
