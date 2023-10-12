using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Windbreaker1SkillHandler : NoblesseSkillHandler
{
    public override int ID => Job.Windbreaker;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.WindbreakerStorm:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
