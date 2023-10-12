using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class PirateViper4SkillHandler : PirateViper3SkillHandler
{
    public override int ID => Job.Viper;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.ViperSuperTransform:
                context.AddTemporaryStat(TemporaryStatType.Morph, context.SkillLevel!.Morph);
                break;
            case Skill.ViperWindBooster:
                context.SetTwoStatePartyBooster(context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
