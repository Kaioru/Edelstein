using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefShadower4SkillHandler : ThiefShadower3SkillHandler
{
    public override int ID => Job.Shadower;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
        }

        return base.HandleSkillUse(context, user);
    }
}
