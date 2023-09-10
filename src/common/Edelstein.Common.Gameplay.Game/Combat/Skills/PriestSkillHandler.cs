using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class PriestSkillHandler : ClericSkillHandler
{
    public override int ID => Job.Priest;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.PriestShiningRay:
                if (context.Random.Next(0, 100) <= context.SkillLevel!.Prop)
                    context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.PriestDispel:
                // TODO dispel
                break;
            case Skill.PriestMysticDoor:
                // TODO dispel
                break;
            case Skill.PriestHolySymbol:
                context.SetTargetParty();
                context.AddTemporaryStat(TemporaryStatType.HolySymbol, context.SkillLevel!.X);
                break;
            case Skill.PriestDoom:
                if (context.Random.Next(0, 100) <= context.SkillLevel!.Prop)
                    context.AddMobTemporaryStat(MobTemporaryStatType.Doom, 1);
                break;
            case Skill.PriestSummonDragon:
                context.AddSummoned(MoveAbilityType.Walk, SummonedAssistType.Attack);
                break;
            case Skill.PriestTeleportMastery:
                context.AddTemporaryStat(TemporaryStatType.TeleportMasteryOn, context.SkillLevel!.X, expire: DateTime.MaxValue);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
