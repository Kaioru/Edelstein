using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserBodyAttackHandler : AbstractUserAttackPacketHandler
    {
        public override AttackType Type => AttackType.Body;
        public override short Operation => (short)PacketRecvOperations.UserBodyAttack;
    }
}
