using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Wildhunter1SkillHandler : CitizenSkillHandler
{
    public override int ID => Job.Wildhunter;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.WildhunterJaguarRiding:
                // TODO jaguar
                context.SetTwoStateRideVehicle(1932015);
                break;
            case Skill.WildhunterCrossbowBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }

    public override Task HandleSkillCancel(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.WildhunterJaguarRiding:
                context.ResetTwoStateRideVehicle();
                break;
        }

        return base.HandleSkillCancel(context, user);
    }
}
