using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ArcherBowmaster4SkillHandler : ArcherBowmaster3SkillHandler
{
    public override int ID => Job.Bowmaster;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BowmasterSharpEyes:
                var cr = context.SkillLevel!.X;
                var cd = context.SkillLevel!.CDMax;
                
                context.AddTemporaryStat(TemporaryStatType.SharpEyes, (cr << 8) + cd);
                break;
            case Skill.BowmasterHamstring:
                context.AddTemporaryStat(TemporaryStatType.HamString, 1);
                break;
            case Skill.BowmasterPhoenix:
                context.ResetSummoned(Skill.RangerSilverHawk);
                context.AddSummoned(MoveAbilityType.Fly, SummonedAssistType.Attack);
                break;
            case Skill.BowmasterConcentration:
                context.AddTemporaryStat(TemporaryStatType.Concentration, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
