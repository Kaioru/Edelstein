using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserSkillPrepareRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserSkillPrepareRequest>
{
    public UserSkillPrepareRequestHandler(IPipeline<FieldOnPacketUserSkillPrepareRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserSkillPrepareRequest;

    protected override FieldOnPacketUserSkillPrepareRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user, 
            reader.ReadInt(), 
            reader.ReadByte(), 
            reader.ReadShort(), 
            reader.ReadByte()
        );
}
