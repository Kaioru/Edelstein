using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface ISkillHandler : IIdentifiable<int>
{
    Task HandleAttack(ISkillContext context, IFieldUser user);
    Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob);
    
    Task HandleSkillUse(ISkillContext context, IFieldUser user);
    Task HandleSkillCancel(ISkillContext context, IFieldUser user);
}
