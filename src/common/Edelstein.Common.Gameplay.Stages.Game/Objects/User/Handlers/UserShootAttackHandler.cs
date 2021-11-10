using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserShootAttackHandler : AbstractUserAttackPacketHandler
    {
        public override AttackType Type => AttackType.Shoot;
        public override short Operation => (short)PacketRecvOperations.UserShootAttack;
    }
}
