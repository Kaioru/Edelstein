using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class Flamewizard1SkillHandler : NoblesseSkillHandler
{
    public override int ID => Job.Flamewizard;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.FlamewizardMagicGuard:
                context.AddTemporaryStat(TemporaryStatType.MagicGuard, context.SkillLevel!.X);
                break;
            case Skill.FlamewizardFlame:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
