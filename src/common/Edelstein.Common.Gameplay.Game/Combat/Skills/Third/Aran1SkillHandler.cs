using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Third;

public class Aran1SkillHandler : LegendSkillHandler
{
    public override int ID => Job.Aran;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        // TODO combo counter
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.AranPolearmBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
