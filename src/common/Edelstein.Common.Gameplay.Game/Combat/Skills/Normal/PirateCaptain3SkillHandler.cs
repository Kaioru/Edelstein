using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class PirateCaptain3SkillHandler : PirateCaptain2SkillHandler
{
    public override int ID => Job.Valkyrie;

    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.ValkyrieGabiota:
                context.ResetSummoned();
                break;
            case Skill.ValkyrieFireBurner:
                context.AddMobBurnedInfo(await user.Damage.CalculateBurnedDamage(
                    user.Character,
                    user.Stats,
                    mob,
                    mob.Stats,
                    context.Skill!.ID,
                    context.SkillLevel!.Level
                ));
                break;
            case Skill.ValkyrieCoolingEffect:
                context.AddMobTemporaryStat(MobTemporaryStatType.Freeze, 1);
                break;
            case Skill.ValkyrieHoming:
                context.SetTwoStateGuidedBullet(context.SkillLevel!.X, mob.ObjectID ?? 0);
                break;
        }
        
        await base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.ValkyrieOctopus:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.Attack);
                break;
            case Skill.ValkyrieGabiota:
                context.AddSummoned(MoveAbilityType.FlyRandom, SummonedAssistType.Attack);
                break;
            case Skill.ValkyrieDice:
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
