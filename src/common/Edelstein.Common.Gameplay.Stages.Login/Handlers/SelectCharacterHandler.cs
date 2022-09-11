using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class SelectCharacterHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ICharacterSelect> _pipeline;

    public SelectCharacterHandler(
        IPipeline<ICharacterSelect> pipeline
    ) =>
        _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.SelectCharacter;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.Completed;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var characterID = reader.ReadInt();

        return _pipeline.Process(new CharacterSelect(user, characterID));
    }
}
