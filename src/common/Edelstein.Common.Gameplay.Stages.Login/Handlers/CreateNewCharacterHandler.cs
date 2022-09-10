using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CreateNewCharacterHandler : AbstractLoginPacketHandler
{
    private IPipeline<ICharacterCreate> _pipeline;

    public CreateNewCharacterHandler(IPipeline<ICharacterCreate> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.CreateNewCharacter;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var name = reader.ReadString();
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        var race = reader.ReadInt();
        var subJob = reader.ReadShort();
        var gender = reader.ReadByte();
        var skin = reader.ReadByte();

        var look = new int[reader.ReadByte()];

        for (var i = 0; i < look.Length; i++)
            look[i] = reader.ReadInt();

        return _pipeline.Process(new CharacterCreate(user, name, race, subJob, gender, skin, look));
    }
}
