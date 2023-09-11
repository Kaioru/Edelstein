using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ArchmageThunderColdSkillHandler : MageThunderColdSkillHandler
{
    public override int ID => Job.ArchmageThunderCold;
    
    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Archmage2IceDemon:
            case Skill.Archmage2Blizzard:
                context.AddMobBurned(await user.Damage.CalculateBurnedDamage(
                    user.Character,
                    user.Stats,
                    mob,
                    mob.Stats,
                    context.Skill!.ID,
                    context.SkillLevel!.Level
                ));
                break;
        }

        await base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Archmage2MapleHero:
                context.SetTargetParty();
                context.AddTemporaryStat(TemporaryStatType.BasicStatUp, context.SkillLevel!.X);
                break;
            case Skill.Archmage2ManaReflection:
                context.AddTemporaryStat(TemporaryStatType.ManaReflection, context.SkillLevel!.X);
                break;
            case Skill.Archmage2Infinity:
                // TODO increase MADR overtime
                context.AddTemporaryStat(TemporaryStatType.Infinity, 1);
                break;
            case Skill.Archmage2Elquines:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                break;
            case Skill.Archmage2HerosWill:
                context.ResetTemporaryStatNegative();
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
