﻿using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Striker1SkillHandler : NoblesseSkillHandler
{
    public override int ID => Job.Striker;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.StrikerDash:
                context.SetTwoStateDashSpeed(context.SkillLevel!.X);
                context.SetTwoStateDashJump(context.SkillLevel!.Y);
                break;
            case Skill.StrikerLightning:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
