﻿using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class WizardFirePoisonSkillHandler : MagicianSkillHandler
{
    public override int ID => Job.WizardFirePoison;
    
    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Wizard1PoisonBreath:
                context.AddMobBurnedInfo(await user.Damage.CalculateBurnedDamage(
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
            case Skill.Wizard1Meditation:
                context.TargetParty();
                break;
            case Skill.Wizard1Slow:
                context.TargetField();
                context.AddMobTemporaryStat(MobTemporaryStatType.Speed, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
