using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface ISkillManager
{
    Task ProcessUserAttackMob(IFieldUser user, IFieldMob mob, IAttackRequest attack, IAttackRequestEntry attackEntry);
    
    Task<bool> ProcessUserAttack(IFieldUser user, IAttackRequest attack);
    Task<bool> ProcessUserSkill(IFieldUser user, int skillID);
}
