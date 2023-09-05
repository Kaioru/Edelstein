using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSkillUseRequestPlug : IPipelinePlug<FieldOnPacketUserSkillUseRequest>
{
    private readonly ITemplateManager<ISkillTemplate> _skillTemplates;
    
    public FieldOnPacketUserSkillUseRequestPlug(ITemplateManager<ISkillTemplate> skillTemplates) 
        => _skillTemplates = skillTemplates;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillUseRequest message)
    {
        var skill = await _skillTemplates.Retrieve(message.SkillID);
        var level = skill?.Levels[message.User.Character.Skills[message.SkillID]?.Level ?? 0];

        if (skill == null || level == null) return;
        if (level.MPCon > message.User.Character.MaxMP) return;

        var stats = new List<Tuple<TemporaryStatType, short>>();
        var expire = DateTime.UtcNow.AddSeconds(level.Time);
        
        if (level.PAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.PAD, level.PAD));
        if (level.PDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.PDD, level.PDD));
        if (level.MAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.MAD, level.MAD));
        if (level.MDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.MDD, level.MDD));

        if (skill.ID is Skill.CrusaderComboAttack or Skill.SoulmasterComboAttack)
            stats.Add(Tuple.Create(TemporaryStatType.ComboCounter, (short)1));
        
        await message.User.ModifyStats(s =>
        {
            if (level.MPCon > 0)
                s.MP -= level.MPCon;
        });
        await message.User.ModifyTemporaryStats(s =>
        {
            s.ResetByReason(message.SkillID);
            foreach (var tuple in stats)
                s.Set(tuple.Item1, tuple.Item2, message.SkillID, expire);
        });
        await message.User.Dispatch(new PacketWriter(PacketSendOperations.SkillUseResult)
            .WriteBool(true)
            .Build());
    }
}
