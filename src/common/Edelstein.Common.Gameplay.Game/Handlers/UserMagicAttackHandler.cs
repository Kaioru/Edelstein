﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserMagicAttackHandler : AbstractUserAttackHandler
{
    public override short Operation => (short)PacketRecvOperations.UserMagicAttack;
    protected override AttackType Type => AttackType.Magic;
    
    public UserMagicAttackHandler(IPipeline<FieldOnPacketUserAttack?> pipeline) : base(pipeline)
    {
    }
}
