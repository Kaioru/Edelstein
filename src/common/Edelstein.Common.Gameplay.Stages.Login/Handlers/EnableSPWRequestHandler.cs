using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Buffers.Bytes;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class EnableSPWRequestHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<IEnableSPWRequest> _pipeline;

    public EnableSPWRequestHandler(IPipeline<IEnableSPWRequest> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.EnableSPWRequest;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW == null;

    public override Task Handle(ILoginStageUser user, IByteReader reader)
    {
        _ = reader.ReadBool(); // Unknown1

        var message = new EnableSPWRequest(
            user,
            reader.ReadInt(),
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadString()
        );

        return _pipeline.Process(message);
    }
}
