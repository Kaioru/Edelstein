using System;
using System.Collections.Generic;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Skills;
using Edelstein.Protocol.Gameplay.Users.Skills.Modify;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Users.Skills.Modify
{
    public class ModifySkillContext : IModifySkillContext, IPacketWritable
    {
        private readonly Character _character;
        private readonly IDictionary<int, CharacterSkillRecord> _modified;

        public ModifySkillContext(Character character)
        {
            _character = character;
            _modified = new Dictionary<int, CharacterSkillRecord>();
        }

        public void Add(int templateID, int increment = 1)
        {
            if (_character.SkillRecord.ContainsKey(templateID))
                Set(templateID, _character.SkillRecord[templateID].Level + increment);
            else Set(templateID, 1);
        }

        public void Set(int templateID, int level, int? masterLevel = null, DateTime? dateExpire = null)
        {
            if (level <= 0 || (GameConstants.IsSkillNeedMasterLevel(templateID) && (masterLevel <= 0)))
            {
                _character.SkillRecord.Remove(templateID);
                return;
            }

            if (!_character.SkillRecord.ContainsKey(templateID))
            {
                var newRecord = new CharacterSkillRecord
                {
                    Level = level,
                    MasterLevel = masterLevel,
                    DateExpire = dateExpire
                };

                _character.SkillRecord.Add(templateID, newRecord);
                _modified.Add(templateID, newRecord);
                return;
            }

            var record = _character.SkillRecord[templateID];

            record.Level = level;
            record.MasterLevel = masterLevel;
            record.DateExpire = dateExpire;

            _modified.Add(templateID, record);
        }

        public void WriteToPacket(IPacketWriter writer)
        {
            writer.WriteShort((short)_modified.Count);
            _modified.ForEach(kv =>
            {
                writer.WriteInt(kv.Key);
                writer.WriteInt(kv.Value.Level);
                writer.WriteInt(kv.Value.MasterLevel ?? 0);
                writer.WriteDateTime(kv.Value.DateExpire ?? GameConstants.Permanent);
            });
        }
    }
}
