using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefNightlord2SkillHandle : Thief1SkillHandler
{
    public override int ID => Job.Assassin;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.AssassinJavelinBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
