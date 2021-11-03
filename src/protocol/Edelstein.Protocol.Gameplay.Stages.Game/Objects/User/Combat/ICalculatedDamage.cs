using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Combat
{
    public interface ICalculatedDamage
    {
        IEnumerable<ICalculatedDamageInfo> CalculatePhysicalDamage(IAttackInfo attack);
        IEnumerable<ICalculatedDamageInfo> CalculateMagicDamage(IAttackInfo attack);
    }
}
