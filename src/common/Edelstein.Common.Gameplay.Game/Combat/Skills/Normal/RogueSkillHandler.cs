using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class RogueSkillHandler : NoviceSkillHandler
{
    public override int ID => Job.Rogue;

    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.RogueDisorder:
                context.AddMobTemporaryStat(MobTemporaryStatType.PAD, context.SkillLevel!.X);
                context.AddMobTemporaryStat(MobTemporaryStatType.PDR, context.SkillLevel!.Y);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.RogueDarkSight:
                context.AddTemporaryStat(TemporaryStatType.DarkSight, context.SkillLevel!.X);
                context.AddTemporaryStat(TemporaryStatType.Speed, -context.SkillLevel!.Y);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
