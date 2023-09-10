using Edelstein.Common.Gameplay.Game.Combat.Damage.Requests;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class SummonedAttackHandler : AbstractPipedFieldSummonedHandler<FieldOnPacketSummonedAttack>
{
    public SummonedAttackHandler(IPipeline<FieldOnPacketSummonedAttack?> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.SummonedAttack;

    protected override FieldOnPacketSummonedAttack? Serialize(IFieldUser user, IFieldSummoned summoned, IPacketReader reader)
        => new(user, summoned, reader.Read(new SummonedAttackRequest()));
}
