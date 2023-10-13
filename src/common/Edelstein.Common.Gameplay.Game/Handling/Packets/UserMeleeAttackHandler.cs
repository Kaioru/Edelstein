using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserMeleeAttackHandler : AbstractUserAttackHandler
{
    public override short Operation => (short)PacketRecvOperations.UserMeleeAttack;
    protected override AttackType Type => AttackType.Melee;
    
    public UserMeleeAttackHandler(IPipeline<FieldOnPacketUserAttack> pipeline) : base(pipeline)
    {
    }
}
