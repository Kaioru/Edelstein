using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserBodyAttackHandler : AbstractUserAttackHandler
{
    public override short Operation => (short)PacketRecvOperations.UserBodyAttack;
    protected override AttackType Type => AttackType.Body;
    
    public UserBodyAttackHandler(IPipeline<FieldOnPacketUserAttack> pipeline) : base(pipeline)
    {
    }
}
