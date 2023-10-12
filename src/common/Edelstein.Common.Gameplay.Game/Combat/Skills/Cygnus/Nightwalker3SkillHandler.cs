using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Nightwalker3SkillHandler : Nightwalker2SkillHandler
{
    public override int ID => Job.Nightwalker3;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            // TODO fix to grenade location
            case Skill.NightwalkerPoisonBomb:
                context.AddAffectedArea(AffectedAreaType.UserSkill);
                context.AddAffectedAreaBurnedInfo();
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.NightwalkerShadowPartner:
                context.AddTemporaryStat(TemporaryStatType.ShadowPartner, 1);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
