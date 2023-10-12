using Edelstein.Common.Gameplay.Game.Objects.Summoned;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class SummonedSkillHandler : AbstractPipedFieldSummonedHandler<FieldOnPacketSummonedSkill>
{
    public SummonedSkillHandler(IPipeline<FieldOnPacketSummonedSkill> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.SummonedSkill;

    protected override FieldOnPacketSummonedSkill? Serialize(IFieldUser user, IFieldSummoned summoned, IPacketReader reader)
        => new(
            user,
            summoned,
            reader.ReadInt(),
            new FieldSummonedMoveAction(reader.ReadByte()),
            reader.ReadByte()
        );
}
