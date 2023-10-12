using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class WarriorPaladin4SkillHandler : WarriorPaladin3SkillHandler
{
    public override int ID => Job.Paladin;

    public override Task HandleAttack(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.PaladinBlast:
                // TODO: instant death chance
                break;
        }

        return base.HandleAttack(context, user);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.PaladinMapleHero:
                context.TargetParty();
                context.AddTemporaryStat(TemporaryStatType.BasicStatUp, context.SkillLevel!.X);
                break;
            case Skill.PaladinStance:
                context.AddTemporaryStat(TemporaryStatType.Stance, context.SkillLevel!.Prop);
                break;
            case Skill.PaladinDivineCharge:
                context.AddTemporaryStat(TemporaryStatType.WeaponCharge, context.SkillLevel!.X);
                break;
            case Skill.PaladinHerosWill:
                context.ResetTemporaryStatNegative();
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
