using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Striker3SkillHandler : Striker2SkillHandler
{
    public override int ID => Job.Striker3;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.StrikerTransform:
                context.AddTemporaryStat(TemporaryStatType.Morph, context.SkillLevel!.Morph);
                break;
            case Skill.StrikerWindBooster:
                context.TargetParty();
                Console.WriteLine(context.SkillLevel?.Time);
                context.SetTwoStatePartyBooster(context.SkillLevel!.X);
                break;
            case Skill.StrikerSpark:
                context.AddTemporaryStat(TemporaryStatType.Spark, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
