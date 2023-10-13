using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class WarriorDarkKnight2SkillHandler : Warrior1SkillHandler
{
    public override int ID => Job.Spearman;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.SpearmanWeaponBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.SpearmanIronWall:
                context.TargetParty();
                break;
            case Skill.SpearmanHyperBody:
                context.TargetParty();
                context.AddTemporaryStat(TemporaryStatType.MaxHP, context.SkillLevel!.X);
                context.AddTemporaryStat(TemporaryStatType.MaxMP, context.SkillLevel!.Y);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
