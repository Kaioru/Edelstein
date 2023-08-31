using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserShootAttackHandler : AbstractUserAttackHandler
{
    public override short Operation => (short)PacketRecvOperations.UserShootAttack;
    protected override AttackType Type => AttackType.Shoot;
    
    public UserShootAttackHandler(IPipeline<FieldOnPacketUserAttack?> pipeline) : base(pipeline)
    {
    }
}
