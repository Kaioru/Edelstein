using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Mechanic2SkillHandler : Mechanic1SkillHandler
{
    public override int ID => Job.Mechanic2;

    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.MechanicEarthSlug:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
            case Skill.MechanicFlamethrowerUp:
                var flamethrowerSkillLevel = user.Stats.SkillLevels[Skill.MechanicGunMastery];
                var flamethrowerLevel = context.Skill[flamethrowerSkillLevel];
                
                context.AddMobBurnedInfo(
                    await user.Damage.CalculateBurnedDamage(
                        user.Character,
                        user.Stats,
                        mob,
                        mob.Stats,
                        context.Skill!.ID,
                        flamethrowerSkillLevel
                    ),
                    interval: TimeSpan.FromSeconds(flamethrowerLevel?.DotInterval ?? 0),
                    expire: DateTime.UtcNow.AddSeconds(flamethrowerLevel?.DotTime ?? 0));
                break;
        }
        
        await base.HandleAttackMob(context, user, mob);
    }
    
    public override async Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.MechanicBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }

        await base.HandleSkillUse(context, user);
    }
}
