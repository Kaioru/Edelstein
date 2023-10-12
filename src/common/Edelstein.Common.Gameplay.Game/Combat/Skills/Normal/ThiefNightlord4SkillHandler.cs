using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefNightlord4SkillHandler : ThiefNightlord3SkillHandler
{
    public override int ID => Job.Nightlord;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
        }

        return base.HandleSkillUse(context, user);
    }
}
