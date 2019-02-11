using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Service.Game.Fields.User.Stats.Modify
{
    public class ModifyTemporaryStatContext
    {
        private readonly FieldUser _user;
        public readonly List<TemporaryStatEntry> ResetOperations;
        public readonly List<TemporaryStatEntry> SetOperations;

        public ModifyTemporaryStatContext(FieldUser user)
        {
            _user = user;
            ResetOperations = new List<TemporaryStatEntry>();
            SetOperations = new List<TemporaryStatEntry>();
        }

        public void Set(TemporaryStatType type, int templateID, short option, DateTime? dateExpire = null)
        {
            var ts = new TemporaryStatEntry
            {
                Type = type,
                TemplateID = templateID,
                Option = option,
                DateExpire = dateExpire
            };

            Reset(type);
            SetOperations.Add(ts);
            _user.TemporaryStat.Entries.Add(ts.Type, ts);
        }

        public void Reset(TemporaryStatType type)
        {
            if (_user.TemporaryStat.Entries.ContainsKey(type))
                ResetOperations.Add(_user.TemporaryStat.Entries[type]);
            _user.TemporaryStat.Entries.Remove(type);
        }

        public void Reset(int templateID)
        {
            _user.TemporaryStat.Entries.Values
                .Where(ts => ts.TemplateID == templateID)
                .ToList()
                .ForEach(ts =>
                {
                    _user.TemporaryStat.Entries.Remove(ts.Type);
                    ResetOperations.Add(ts);
                });
        }
    }
}