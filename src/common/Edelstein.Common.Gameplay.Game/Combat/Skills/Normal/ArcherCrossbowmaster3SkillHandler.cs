using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ArcherCrossbowmaster3SkillHandler : ArcherCrossbowmaster2SkillHandler
{
    public override int ID => Job.Sniper;
    
    public override async Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.SniperIceShot:
                context.AddMobTemporaryStat(MobTemporaryStatType.Freeze, 1);
                break;
            case Skill.SniperGoldenEagle:
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
            case Skill.SniperPuppet:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.None);
                break;
            case Skill.SniperGoldenEagle:
                context.ResetSummoned(Skill.CrossbowmasterFreezer);
                context.AddSummoned(MoveAbilityType.Fly, SummonedAssistType.Attack);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
