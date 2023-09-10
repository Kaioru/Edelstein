using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class Flamewizard3SkillHandler : Flamewizard2SkillHandler
{
    public override int ID => Job.Flamewizard3;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.FlamewizardSeal:
                if (context.Random.Next(0, 100) <= context.SkillLevel!.Prop)
                    context.AddMobTemporaryStat(MobTemporaryStatType.Seal, 1);
                break;
            case Skill.FlamewizardIfrit:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                break;
            case Skill.FlamewizardFlameGear:
                // TODO affected area
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
