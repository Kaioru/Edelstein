using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class PirateViper3SkillHandler : PirateViper2SkillHandler
{
    public override int ID => Job.Buccaneer;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BuccaneerTransform:
                context.AddTemporaryStat(TemporaryStatType.Morph, context.SkillLevel!.Morph);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
