using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserMeleeAttackHandler : AbstractUserAttackPacketHandler
    {
        public override AttackType Type => AttackType.Melee;
        public override short Operation => (short)PacketRecvOperations.UserMeleeAttack;
    }
}
