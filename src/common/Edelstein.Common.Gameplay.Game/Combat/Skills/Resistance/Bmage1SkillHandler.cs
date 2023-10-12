using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Bmage1SkillHandler : CitizenSkillHandler
{
    public override int ID => Job.Bmage;
    
    public override async Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageAuraDark:
                context.TargetParty();
                context.ResetTemporaryStatAuras();

                var auraSkill = context.Skill;
                var auraLevel = context.SkillLevel;

                if (user.Stats.SkillLevels[Skill.BmageAuraDarkAdvanced] > 0)
                {
                    auraSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.BmageAuraDarkAdvanced);
                    auraLevel = auraSkill?[user.Stats.SkillLevels[Skill.BmageAuraDarkAdvanced]];
                }

                if (auraSkill != null && auraLevel != null)
                {
                    context.AddTemporaryStat(TemporaryStatType.DarkAura, auraLevel.X, auraSkill.ID);
                    await user.ModifyTemporaryStats(s => s.Set(
                        TemporaryStatType.Aura, 
                        context.SkillLevel!.Level,
                        context.Skill!.ID
                    ));
                }
                break;
        }

        await base.HandleSkillUse(context, user);
    }
}
