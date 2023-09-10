using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface ISkillManager : 
    IRepositoryMethodInsert<int, ISkillHandler>,
    IRepositoryMethodDelete<int, ISkillHandler>,
    IRepositoryMethodRetrieve<int, ISkillHandler>,
    IRepositoryMethodRetrieveAll<int, ISkillHandler>
{
    Task<bool> Check(IFieldUser user, int skillID);
    
    Task HandleAttack(IFieldUser user, int skillID, bool IsHitMob);
    Task HandleAttackMob(IFieldUser user, IFieldMob mob, int skillID, int damage);
    
    Task HandleSkillUse(IFieldUser user, int skillID);
    Task HandleSkillCancel(IFieldUser user, int skillID);
}
