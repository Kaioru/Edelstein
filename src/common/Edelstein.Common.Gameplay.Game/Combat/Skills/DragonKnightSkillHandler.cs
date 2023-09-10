using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class DragonKnightSkillHandler : SpearmanSkillHandler
{
    public override int ID => Job.DragonKnight;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.DragonknightMagicCrash:
                context.SetTargetField();
                context.AddMobTemporaryStat(MobTemporaryStatType.MagicCrash, 1);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
