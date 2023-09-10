using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class MagicianSkillHandler : NoviceSkillHandler
{
    public override int ID => Job.Magician;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.MagicianMagicGuard:
                context.AddTemporaryStat(TemporaryStatType.MagicGuard, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
