using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class MagicianBishop4SkillHandler : MagicianBishop3SkillHandler
{
    public override int ID => Job.Bishop;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BishopMapleHero:
                context.TargetParty();
                context.AddTemporaryStat(TemporaryStatType.BasicStatUp, context.SkillLevel!.X);
                break;
            case Skill.BishopManaReflection:
                context.AddTemporaryStat(TemporaryStatType.ManaReflection, context.SkillLevel!.X);
                break;
            case Skill.BishopInfinity:
                // TODO increase MADR overtime
                context.AddTemporaryStat(TemporaryStatType.Infinity, 1);
                break;
            case Skill.BishopBahamut:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                context.ResetSummoned(Skill.PriestSummonDragon);
                break;
            case Skill.BishopResurrection:
                // TODO resurrection
                break;
            case Skill.BishopHerosWill:
                context.ResetTemporaryStatNegative();
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
