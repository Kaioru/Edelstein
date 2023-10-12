using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Bmage2SkillHandler : Bmage1SkillHandler
{
    public override int ID => Job.Bmage2;
    
    public override async Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageAuraBlue:
            {
                if (user.Character.TemporaryStats[TemporaryStatType.SuperBody] != null)
                    return;
                
                context.TargetParty();
                context.ResetTemporaryStatAuras();

                var auraSkill = context.Skill;
                var auraLevel = context.SkillLevel;

                if (user.Stats.SkillLevels[Skill.BmageAuraBlueAdvanced] > 0)
                {
                    auraSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.BmageAuraBlueAdvanced);
                    auraLevel = auraSkill?[user.Stats.SkillLevels[Skill.BmageAuraBlueAdvanced]];
                }

                if (auraSkill != null && auraLevel != null)
                {
                    context.AddTemporaryStat(TemporaryStatType.BlueAura, auraLevel.Level, auraSkill.ID);
                    await user.ModifyTemporaryStats(s => s.Set(
                        TemporaryStatType.Aura,
                        context.SkillLevel!.Level,
                        context.Skill!.ID
                    ));
                }

                break;
            }
            case Skill.BmageAuraYellow:
            {
                if (user.Character.TemporaryStats[TemporaryStatType.SuperBody] != null)
                    return;
                
                context.TargetParty();
                context.ResetTemporaryStatAuras();

                var auraSkill = context.Skill;
                var auraLevel = context.SkillLevel;

                if (user.Stats.SkillLevels[Skill.BmageAuraYellowAdvanced] > 0)
                {
                    auraSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.BmageAuraYellowAdvanced);
                    auraLevel = auraSkill?[user.Stats.SkillLevels[Skill.BmageAuraYellowAdvanced]];
                }

                if (auraSkill != null && auraLevel != null)
                {
                    context.AddTemporaryStat(TemporaryStatType.YellowAura, auraLevel.Level, auraSkill.ID);
                    await user.ModifyTemporaryStats(s => s.Set(
                        TemporaryStatType.Aura,
                        context.SkillLevel!.Level,
                        context.Skill!.ID
                    ));
                }

                break;
            }
            case Skill.BmageBloodDrain:
                // TODO
                break;
            case Skill.BmageStaffBooster:
                Console.WriteLine(context.SkillLevel!.X);
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }

        await base.HandleSkillUse(context, user);
    }
}
