using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Mechanic1SkillHandler : CitizenSkillHandler
{
    public override int ID => Job.Mechanic;
    
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
