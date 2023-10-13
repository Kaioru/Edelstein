using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Mechanic1SkillHandler : CitizenSkillHandler
{
    public override int ID => Job.Mechanic;
    
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
            case Skill.MechanicHn07:
                context.SetTwoStateRideVehicle(1932016);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }

    public override Task HandleSkillCancel(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.MechanicHn07:
                context.ResetTwoStateRideVehicle();
                break;
        }

        return base.HandleSkillCancel(context, user);
    }
}
