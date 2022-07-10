using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserMagicAttackHandler : AbstractUserAttackPacketHandler
    {
        public override AttackType Type => AttackType.Magic;
        public override short Operation => (short)PacketRecvOperations.UserMagicAttack;
    }
}
