using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking
{
    public interface IAttackInfo
    {
        IFieldObjUser User { get; }
        IFieldObjMob Mob { get; }

        int WeaponID { get; }
        int BulletID { get; }

        int SkillID { get; }
        int SkillLevel { get; }

        int Keydown { get; }
    }
}
