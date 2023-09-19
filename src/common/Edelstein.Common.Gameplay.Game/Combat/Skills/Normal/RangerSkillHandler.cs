﻿using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class RangerSkillHandler : HunterSkillHandler
{
    public override int ID => Job.Ranger;
    
    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.RangerFireShot:
                context.AddMobBurnedInfo(await user.Damage.CalculateBurnedDamage(
                    user.Character,
                    user.Stats,
                    mob,
                    mob.Stats,
                    context.Skill!.ID,
                    context.SkillLevel!.Level
                ));
                break;
            case Skill.RangerSilverHawk:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
        }
        
        await base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.RangerPuppet:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.None);
                break;
            case Skill.RangerSilverHawk:
                context.ResetSummoned(Skill.BowmasterPhoenix);
                context.AddSummoned(MoveAbilityType.Fly, SummonedAssistType.Attack);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
