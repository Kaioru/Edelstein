using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class AdminSuperGMSkillHandler : AdminSkillHandler
{
    public override int ID => Job.AdminSuperGM;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.AdminDispel:
                context.SetTargetField();
                break;
            case Skill.AdminSuperHaste:
                context.SetTargetField();
                break;
            case Skill.AdminHolySymbol:
                context.SetTargetField();
                context.AddTemporaryStat(TemporaryStatType.ExpBuffRate, context.SkillLevel!.X);
                break;
            case Skill.AdminBless:
                context.SetTargetField();
                break;
            case Skill.AdminHide:
                break;
            case Skill.AdminResurrection:
                context.SetTargetField();
                break;
            case Skill.AdminHyperBody:
                context.SetTargetField();
                context.AddTemporaryStat(TemporaryStatType.MaxHP, context.SkillLevel!.X);
                context.AddTemporaryStat(TemporaryStatType.MaxMP, context.SkillLevel!.Y);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
