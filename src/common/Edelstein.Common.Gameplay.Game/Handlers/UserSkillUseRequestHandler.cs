using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserSkillUseRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserSkillUseRequest>
{
    public UserSkillUseRequestHandler(IPipeline<FieldOnPacketUserSkillUseRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserSkillUseRequest;

    protected override FieldOnPacketUserSkillUseRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(user, reader.Skip(4).ReadInt());
}
