using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat;

public abstract class AbstractSkillHandler : ISkillHandler
{
    public abstract int ID { get; }
    
    public virtual Task HandleAttack(ISkillContext context, IFieldUser user)
        => Task.CompletedTask;

    public virtual Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
        => Task.CompletedTask;
    
    public virtual async Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        await this.HandleSkillUseBasic(context, user);
        await this.HandleSkillUseBeginner(context, user);
    }

    public virtual Task HandleSkillCancel(ISkillContext context, IFieldUser user)
        => Task.CompletedTask;
}
