using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserSkillCancelRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserSkillCancelRequest>
{
    public UserSkillCancelRequestHandler(IPipeline<FieldOnPacketUserSkillCancelRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserSkillCancelRequest;

    protected override FieldOnPacketUserSkillCancelRequest? Serialize(IFieldUser user, IPacketReader reader) 
        => new(user, reader.ReadInt());
}
