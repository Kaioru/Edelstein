using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class PirateViper2SkillHandler : Pirate1SkillHandler
{
    public override int ID => Job.Infighter;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.InfighterKnuckleBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.InfighterOakCask:
                context.AddTemporaryStat(TemporaryStatType.Morph, context.SkillLevel!.Morph);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
