using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Modify;

public class ModifySkillContext : IModifySkillContext
{
    private readonly ICharacter _character;
    private readonly IDictionary<int, ISkillRecord> _records;

    public ModifySkillContext(ICharacter character)
    {
        _character = character;
        _records = new Dictionary<int, ISkillRecord>();
    }

    public void Add(int templateID, int increment = 1)
        => Set(templateID, (_character.Skills[templateID]?.Level ?? 0) + increment);

    public void Add(ISkillTemplate template, int increment = 1)
        => Add(template.ID, increment);

    public void Set(int templateID, int level, int? masterLevel = null, DateTime? dateExpire = null)
    {
        var record = _character.Skills[templateID];
        
        if (record == null)
        {
            var newRecord = new SkillRecord
            {
                Level = level,
                MasterLevel = masterLevel,
                DateExpire = dateExpire
            };

            _character.Skills.Records.Add(templateID, newRecord);
            _records.Add(templateID, newRecord);
            return;
        }
        
        record.Level = level;
        record.MasterLevel = masterLevel;
        record.DateExpire = dateExpire;

        _records.Add(templateID, record);
    }
    
    public void Set(ISkillTemplate template, int level, int? masterLevel = null, DateTime? dateExpire = null) 
        => Set(template.ID, level, masterLevel, dateExpire);

    public void ResetByTemplate(int templateID) => Set(templateID, 0);
    public void ResetByTemplate(ISkillTemplate template) => ResetByTemplate(template.ID);
    public void ResetByJobLevel(int jobLevel)
    {
        foreach (var kv in _character.Skills.Records
                     .Where(kv => JobConstants.GetJobLevel(jobLevel) == jobLevel)
                     .ToList())
            Set(kv.Key, 0);
    }

    public void ResetAll()
    {
        foreach (var kv in _character.Skills.Records.ToList())
            Set(kv.Key, 0);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteShort((short)_records.Count);
        foreach (var kv in _records)
        {
            writer.WriteInt(kv.Key);
            writer.WriteInt(kv.Value.Level);
            writer.WriteInt(kv.Value.MasterLevel ?? 0);
            writer.WriteDateTime(kv.Value.DateExpire ?? DateTime.FromFileTimeUtc(150842304000000000));
        }
    }
}
