using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CreateNewCharacterHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ICreateNewCharacter> _pipeline;

    public CreateNewCharacterHandler(IPipeline<ICreateNewCharacter> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.CreateNewCharacter;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new CreateNewCharacter(
            user,
            reader.ReadString(),
            reader.ReadInt(),
            reader.ReadShort(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
