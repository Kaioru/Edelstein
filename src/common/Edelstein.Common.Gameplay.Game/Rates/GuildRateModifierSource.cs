using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Rates;

namespace Edelstein.Common.Gameplay.Game.Rates;

public sealed class GuildRateModifierSource : IRateModifierSource
{
    private static readonly int[] GuildSkillIds = [Skill.GuildMesoup, Skill.GuildExperienceup];

    public async ValueTask<IReadOnlyList<IRateModifier>> GetModifiersAsync(RateType type, IRateContext context)
    {
        var user = context.User;
        if (user == null)
            return [];

        IReadOnlyList<IRateModifier> result = [];
        var skills = user.Character.Skills;

        foreach (var skillId in GuildSkillIds)
        {
            var level = skills[skillId]?.Level ?? 0;
            if (level <= 0) continue;

            var template = await user.StageUser.Context.Templates.Skill.Retrieve(skillId);
            var levelTemplate = template?[level];
            if (levelTemplate == null) continue;

            var value = type switch
            {
                RateType.Exp => levelTemplate.EXPr,
                RateType.Meso => levelTemplate.MESOr,
                _ => 0
            };

            RateModifierBuilder.TryAddPercent(ref result, $"guild-{type.ToString().ToLowerInvariant()}-{skillId}", value);
        }

        return result;
    }
}
