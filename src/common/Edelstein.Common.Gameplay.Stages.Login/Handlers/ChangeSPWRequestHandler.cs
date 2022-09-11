using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class ChangeSPWRequestHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ISPWChange> _pipeline;

    public ChangeSPWRequestHandler(
        IPipeline<ISPWChange> pipeline
    ) =>
        _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.ChangeSPWRequest;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW != null;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var spwCurrent = reader.ReadString();
        var spwNew = reader.ReadString();

        return _pipeline.Process(new SPWChange(user, spwCurrent, spwNew));
    }
}
