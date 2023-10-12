using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class MagicianBishop2SkillHandler : Magician1SkillHandler
{
    public override int ID => Job.Cleric;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.ClericHeal:
                context.TargetParty();
                context.SetRecoverHP();
                break;
            case Skill.ClericInvincible:
                context.AddTemporaryStat(TemporaryStatType.Invincible, context.SkillLevel!.Level);
                break;
            case Skill.ClericBless:
                context.TargetParty();
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
