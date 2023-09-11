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
                context.TargetField();
                context.SetRecoverHP(99999);
                context.SetRecoverHP(99999);
                context.ResetTemporaryStatNegative();
                break;
            case Skill.AdminSuperHaste:
                context.TargetField();
                break;
            case Skill.AdminHolySymbol:
                context.TargetField();
                context.AddTemporaryStat(TemporaryStatType.ExpBuffRate, context.SkillLevel!.X);
                break;
            case Skill.AdminBless:
                context.TargetField();
                break;
            case Skill.AdminHide:
                break;
            case Skill.AdminResurrection:
                context.TargetField();
                break;
            case Skill.AdminHyperBody:
                context.TargetField();
                context.AddTemporaryStat(TemporaryStatType.MaxHP, context.SkillLevel!.X);
                context.AddTemporaryStat(TemporaryStatType.MaxMP, context.SkillLevel!.Y);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
