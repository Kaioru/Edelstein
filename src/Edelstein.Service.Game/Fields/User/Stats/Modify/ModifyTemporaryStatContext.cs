using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Service.Game.Fields.User.Stats.Modify
{
    public class ModifyTemporaryStatContext
    {
        private readonly FieldUser _user;
        public IDictionary<TemporaryStatType, TemporaryStat> ResetOperations { get; }
        public IDictionary<TemporaryStatType, TemporaryStat> SetOperations { get; }

        public ModifyTemporaryStatContext(FieldUser user)
        {
            _user = user;
            ResetOperations = new Dictionary<TemporaryStatType, TemporaryStat>();
            SetOperations = new Dictionary<TemporaryStatType, TemporaryStat>();
        }

        public void Set(TemporaryStatType type, int templateID, short option, DateTime? dateExpire = null)
        {
            var ts = new TemporaryStat
            {
                Type = type,
                TemplateID = templateID,
                Option = option,
                DateExpire = dateExpire
            };

            Reset(type);
            SetOperations[ts.Type] = ts;
            _user.TemporaryStats.Add(ts.Type, ts);
        }

        public void Reset(TemporaryStatType type)
        {
            if (_user.TemporaryStats.ContainsKey(type))
                ResetOperations[type] = _user.TemporaryStats[type];
            _user.TemporaryStats.Remove(type);
        }

        public void Reset(int templateID)
        {
            _user.TemporaryStats.Values
                .Where(ts => ts.TemplateID == templateID)
                .ToList()
                .ForEach(ts => Reset(ts.Type));
        }
    }
}