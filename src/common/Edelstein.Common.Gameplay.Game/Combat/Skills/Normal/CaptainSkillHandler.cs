using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class CaptainSkillHandler : ValkyrieSkillHandler
{
    public override int ID => Job.Captain;

    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.CaptainMindControl:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Dazzle, user.Character.ID);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.CaptainBattleship:
                context.SetTwoStateRideVehicle(1932000);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }

    public override Task HandleSkillCancel(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.CaptainBattleship:
                context.ResetTwoStateRideVehicle();
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
