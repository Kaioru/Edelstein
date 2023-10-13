using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Third;

public class Evan7SkillHandler : Evan6SkillHandler
{
    public override int ID => Job.Evan7;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.EvanBreath:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
            case Skill.EvanKillingWing:
                // TODO
                break;
        }

        return base.HandleAttackMob(context, user, mob);
    }
}
