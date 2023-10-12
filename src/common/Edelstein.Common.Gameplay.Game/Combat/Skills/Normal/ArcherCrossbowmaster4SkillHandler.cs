using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ArcherCrossbowmaster4SkillHandler : ArcherCrossbowmaster3SkillHandler
{
    public override int ID => Job.Crossbowmaster;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.CrossbowmasterSharpEyes:
                var cr = context.SkillLevel!.X;
                var cd = context.SkillLevel!.CDMax;
                
                context.AddTemporaryStat(TemporaryStatType.SharpEyes, (cr << 8) + cd);
                break;
            case Skill.CrossbowmasterBlind:
                context.AddTemporaryStat(TemporaryStatType.Blind, 1);
                break;
            case Skill.CrossbowmasterFreezer:
                context.ResetSummoned(Skill.SniperGoldenEagle);
                context.AddSummoned(MoveAbilityType.Fly, SummonedAssistType.Attack);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
