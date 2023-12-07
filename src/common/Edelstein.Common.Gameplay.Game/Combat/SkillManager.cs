using Edelstein.Common.Constants;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Combat;

public sealed class SkillManager : 
    Repository<int, ISkillHandler>, 
    ISkillManager
{
    private readonly ITemplateManager<ISkillTemplate> _skills;
    
    public SkillManager(
        ITemplateManager<ISkillTemplate> skills,
        IEnumerable<ISkillHandler> handlers
    )
    {
        _skills = skills;
        foreach (var handler in handlers)
            Insert(handler).Wait();
    }

    public Task<bool> Check(IFieldUser user, int skillID) => Task.FromResult(true);
    
    public async Task HandleAttack(IFieldUser user, int skillID, bool IsHitMob)
    {
        var context = await CreateContext(user, skillID, user.Stats.SkillLevels[skillID], isHitMob: IsHitMob);
        if (context == null) return;
        var handler = await Retrieve(user.Character.Job);
        if (handler == null) return;
        await handler.HandleAttack(context, user);
        await context.Execute();
    }

    public async Task HandleAttackMob(IFieldUser user, IFieldMob mob, int skillID, int damage, IPoint2D positionHit)
    {
        if (damage == 0) return;
        await mob.Damage(damage, user, positionHit);
        var context = await CreateContext(user, skillID, user.Stats.SkillLevels[skillID], mob: mob);
        if (context == null) return;
        var handler = await Retrieve(user.Character.Job);
        if (handler == null) return;
        await handler.HandleAttackMob(context, user, mob);
        await context.Execute();
    }
    
    public async Task HandleSkillUse(IFieldUser user, int skillID)
    {
        var context = await CreateContext(user, skillID, user.Stats.SkillLevels[skillID]);
        if (context == null) return;
        var handler = await Retrieve(user.Character.Job);
        if (handler == null) return;
        await handler.HandleSkillUse(context, user);
        await context.Execute();
    }

    public async Task HandleSkillCancel(IFieldUser user, int skillID)
    {
        switch (skillID)
        {
            case Skill.BmageAuraDark:
                if (user.Character.TemporaryStats[TemporaryStatType.SuperBody] != null) return;
                await user.ModifyTemporaryStats(s => s.ResetByType(TemporaryStatType.DarkAura));
                await user.ModifyTemporaryStats(s => s.ResetByType(TemporaryStatType.Aura));
                break;
            case Skill.BmageAuraBlue:
                if (user.Character.TemporaryStats[TemporaryStatType.SuperBody] != null) return;
                await user.ModifyTemporaryStats(s => s.ResetByType(TemporaryStatType.BlueAura));
                await user.ModifyTemporaryStats(s => s.ResetByType(TemporaryStatType.Aura));
                break;
            case Skill.BmageAuraYellow:
                if (user.Character.TemporaryStats[TemporaryStatType.SuperBody] != null) return;
                await user.ModifyTemporaryStats(s => s.ResetByType(TemporaryStatType.YellowAura));
                await user.ModifyTemporaryStats(s => s.ResetByType(TemporaryStatType.Aura));
                break;
            default:
                await user.ModifyTemporaryStats(s => s.ResetByReason(skillID));
                break;
        }
        
        var context = await CreateContext(user, skillID, user.Stats.SkillLevels[skillID]);
        if (context == null) return;
        var handler = await Retrieve(user.Character.Job);
        if (handler == null) return;
        await handler.HandleSkillCancel(context, user);
        await context.Execute();
    }

    private async Task<SkillContext?> CreateContext(IFieldUser user, int? skillID, int? skillLevel, bool isHitMob = false, IFieldMob? mob = null)
    {
        if (skillID == null || skillLevel == null)
            return new SkillContext(
                user,
                null,
                null,
                isHitMob || mob != null,
                mob
            );
        
        var skill = await _skills.Retrieve(skillID.Value);
        var level = skill?[skillLevel.Value];

        return new SkillContext(
            user,
            skill,
            level,
            isHitMob || mob != null,
            mob
        );
    }
}
