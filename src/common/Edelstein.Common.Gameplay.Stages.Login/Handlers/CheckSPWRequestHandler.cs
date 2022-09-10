using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckSPWRequestHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ISPWCheck> _pipeline;

    public CheckSPWRequestHandler(
        IPipeline<ISPWCheck> pipeline
    ) =>
        _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.CheckSPWRequest;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW != null;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        Console.WriteLine("EXECUTING");
        var spw = reader.ReadString();

        return _pipeline.Process(new SPWCheck(user, spw));
    }
}
