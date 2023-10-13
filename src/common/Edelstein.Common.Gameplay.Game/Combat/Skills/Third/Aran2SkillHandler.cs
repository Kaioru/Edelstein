using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Third;

public class Aran2SkillHandler : Aran1SkillHandler
{
    public override int ID => Job.Aran2;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.AranBodyPressure:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.AranBodyPressure:
                context.AddTemporaryStat(TemporaryStatType.BodyPressure, 1);
                break;
            case Skill.AranComboDrain:
                context.AddTemporaryStat(TemporaryStatType.ComboDrain, 1);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
