using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface ISkillManager
{
    Task ProcessUserAttackMob(IFieldUser user, IFieldMob mob, int skillID, int damage);
    
    Task<bool> ProcessUserAttack(IFieldUser user, int skillID, bool isHitAnyMob);
    Task<bool> ProcessUserSkill(IFieldUser user, int skillID);
}
