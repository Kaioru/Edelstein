using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Flamewizard3SkillHandler : Flamewizard2SkillHandler
{
    public override int ID => Job.Flamewizard3;

    public override Task HandleAttack(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.FlamewizardFlameGear:
                context.AddAffectedArea(AffectedAreaType.UserSkill);
                context.AddAffectedAreaBurnedInfo();
                break;
        }
        
        return base.HandleAttack(context, user);
    }

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.FlamewizardSeal:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Seal, 1);
                break;
            case Skill.FlamewizardIfrit:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
