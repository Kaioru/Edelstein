using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    public class AttackInfo : IAttackInfo
    {
        public IFieldObjUser User { get;  }
        public IFieldObjMob Mob { get;  }

        public int WeaponID { get; init; }
        public int BulletID { get; init; }

        public int SkillID { get; init; }
        public int SkillLevel { get; init; }

        public int Keydown { get; init; }

        public AttackInfo(IFieldObjUser user, IFieldObjMob mob)
        {
            User = user;
            Mob = mob;
        }

    }
}
