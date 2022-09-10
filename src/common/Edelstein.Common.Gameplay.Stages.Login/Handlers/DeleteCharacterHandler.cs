using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class DeleteCharacterHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ICharacterDelete> _pipeline;

    public DeleteCharacterHandler(IPipeline<ICharacterDelete> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.DeleteCharacter;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW != null;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var spw = reader.ReadString();
        var characterID = reader.ReadInt();

        return _pipeline.Process(new CharacterDelete(user, spw, characterID));
    }
}
