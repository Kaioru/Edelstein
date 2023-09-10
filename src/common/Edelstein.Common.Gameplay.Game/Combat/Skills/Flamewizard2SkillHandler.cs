using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class Flamewizard2SkillHandler : Flamewizard1SkillHandler
{
    public override int ID => Job.Flamewizard2;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.FlamewizardMeditation:
                context.SetTargetParty();
                break;
            case Skill.FlamewizardSlow:
                context.SetTargetField();
                context.AddMobTemporaryStat(MobTemporaryStatType.Speed, context.SkillLevel!.X);
                break;
            case Skill.FlamewizardMagicBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.FlamewizardElementalReset:
                context.AddTemporaryStat(TemporaryStatType.ElementalReset, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
