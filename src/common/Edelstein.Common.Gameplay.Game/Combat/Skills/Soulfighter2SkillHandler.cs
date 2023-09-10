using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class Soulfighter2SkillHandler : Soulfighter1SkillHandler
{
    public override int ID => Job.Soulfighter2;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.SoulmasterSwordBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
