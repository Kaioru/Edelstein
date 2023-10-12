using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefDual4SkillHandler : ThiefDual3SkillHandler
{
    public override int ID => Job.Dual4;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Dual4MirrorImaging:
                context.AddTemporaryStat(TemporaryStatType.ShadowPartner, 1);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}

