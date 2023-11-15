using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Mechanic3SkillHandler : Mechanic2SkillHandler
{
    public override int ID => Job.Mechanic3;
    
    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.MechanicRocketPunch:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
        }
        
        await base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.MechanicVelocityControler:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.None);
                break;
            case Skill.MechanicHealingRobotHLx:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.None);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
