using System;
using System.Collections.Generic;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Entities.Characters.Records;
using Edelstein.Network.Packets;
using MoreLinq;

namespace Edelstein.Core.Gameplay.Skills
{
    public class ModifySkillContext
    {
        private readonly Character _character;
        private readonly IDictionary<int, SkillRecord> _records;

        public ModifySkillContext(Character character)
        {
            _character = character;
            _records = new Dictionary<int, SkillRecord>();
        }

        public void Add(int skill, int masterLevel = 0, DateTime? dateExpire = null)
        {
            if (_character.SkillRecord.ContainsKey(skill))
                Set(skill, _character.SkillRecord[skill].Level + 1, masterLevel, dateExpire);
            else
                Set(skill, masterLevel: masterLevel, dateExpire: dateExpire);
        }

        public void Set(int skill, int? level = null, int masterLevel = 0, DateTime? dateExpire = null)
        {
            if (!_character.SkillRecord.ContainsKey(skill))
            {
                var newRecord = new SkillRecord
                {
                    Level = level ?? 1,
                    MasterLevel = masterLevel,
                    DateExpire = dateExpire
                };

                _character.SkillRecord.Add(skill, newRecord);
                _records.Add(skill, newRecord);
                return;
            }

            var record = _character.SkillRecord[skill];

            if (level <= 0 && masterLevel <= 0) _character.SkillRecord.Remove(skill);

            record.Level = level ?? record.Level;
            record.MasterLevel = masterLevel;
            record.DateExpire = dateExpire;
            _records.Add(skill, record);
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<short>((short) _records.Count);
            _records.ForEach(kv =>
            {
                packet.Encode<int>(kv.Key);
                packet.Encode<int>(kv.Value.Level);
                packet.Encode<int>(kv.Value.MasterLevel);
                packet.Encode<DateTime>(kv.Value.DateExpire ?? ItemConstants.Permanent);
            });
        }
    }
}