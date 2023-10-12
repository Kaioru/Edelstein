using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Bmage3SkillHandler : Bmage2SkillHandler
{
    public override int ID => Job.Bmage3;

    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        if (user.Character.TemporaryStats[TemporaryStatType.Revive] != null)
        {
            var reviveSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.BmageRevive);
            var reviveLevel = reviveSkill?.Levels[user.Stats.SkillLevels[Skill.BmageRevive]];

            if (reviveLevel != null && context.Random.Next(0, 100) <= reviveLevel.Prop)
                context.AddSummoned(
                    MoveAbilityType.WalkRandom, 
                    SummonedAssistType.Attack, 
                    Skill.BmageRevive, 
                    reviveLevel.Level, 
                    true,
                    DateTime.UtcNow.AddSeconds(reviveLevel.Time),
                    mob.Position
                );
        }

        await base.HandleAttackMob(context, user, mob);
    }

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageConversion:
                context.AddTemporaryStat(TemporaryStatType.Conversion, context.SkillLevel!.X);
                break;
            case Skill.BmageSuperBody:
            case Skill.BmageSuperBodyDark:
            case Skill.BmageSuperBodyBlue:
            case Skill.BmageSuperBodyYellow:
                // TODO
                break;
            case Skill.BmageRevive:
                // TODO
                context.AddTemporaryStat(TemporaryStatType.Revive, context.SkillLevel!.X);
                break;
            case Skill.BmageTeleportMastery:
                context.AddTemporaryStat(TemporaryStatType.TeleportMasteryOn, context.SkillLevel!.X, expire: DateTime.MaxValue);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
