using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketSummonedSkillPlug : IPipelinePlug<FieldOnPacketSummonedSkill>
{
    private readonly ITemplateManager<ISkillTemplate> _skillTemplates;
    
    public FieldOnPacketSummonedSkillPlug(ITemplateManager<ISkillTemplate> skillTemplates) 
        => _skillTemplates = skillTemplates;

    public async Task Handle(IPipelineContext ctx, FieldOnPacketSummonedSkill message)
    {
        var skill = await _skillTemplates.Retrieve(message.SkillID);
        var level = skill?.Levels[message.User.Stats.SkillLevels[message.SkillID]];
        
        if (skill == null || level == null) return;
        if (!(message.Summoned.SkillID == Skill.DarkknightBeholder && message.SkillID == Skill.DarkknightBeholdersBuff)) return;
        var stats = new List<Tuple<TemporaryStatType, short>>();
        var expire = DateTime.UtcNow.AddSeconds(level.Time);
        
        if (level.EPAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EPAD, level.EPAD));
        if (level.EPDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EPDD, level.EPDD));
        if (level.EMDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EMDD, level.EMDD));
        if (level.ACC > 0)
            stats.Add(Tuple.Create(TemporaryStatType.ACC, level.ACC));
        if (level.EVA > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EVA, level.EVA));

        await message.User.ModifyTemporaryStats(s =>
        {
            s.ResetByReason(message.SkillID);
            foreach (var tuple in stats)
                s.Set(tuple.Item1, tuple.Item2, message.SkillID, expire);
        });
    }
}
