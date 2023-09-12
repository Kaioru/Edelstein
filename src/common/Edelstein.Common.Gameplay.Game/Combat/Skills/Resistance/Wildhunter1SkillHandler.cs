using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Wildhunter1SkillHandler : CitizenSkillHandler
{
    public override int ID => Job.Wildhunter;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.WildhunterJaguarRiding:
                context.SetTwoStateRideVehicle(1932015);
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
