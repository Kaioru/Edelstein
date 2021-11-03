using System;
using System.Linq;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats.Temporary;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Modify;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats.Modify
{
    public class ModifySecondaryStatContext : IModifySecondaryStatContext
    {
        private readonly SecondaryStats _stats;

        private readonly SecondaryStats _setHistory;
        private readonly SecondaryStats _resetHistory;

        public ISecondaryStats SetHistory => _setHistory;
        public ISecondaryStats ResetHistory => _resetHistory;

        public ModifySecondaryStatContext(SecondaryStats stats)
        {
            _stats = stats;

            _setHistory = new SecondaryStats();
            _resetHistory = new SecondaryStats();
        }

        public void Set(ITemporaryStat stat)
        {
            Reset(stat);

            _setHistory.Stats[stat.ID] = stat;
            _stats.Stats[stat.ID] = stat;
        }

        public void Set(
            SecondaryStatType type,
            int value,
            int reason,
            DateTime? dateExpire = null
        )
            => Set(new TemporaryStat
            {
                ID = type,
                Value = value,
                Reason = reason,
                DateExpire = dateExpire
            });

        public void Reset(ITemporaryStat stat)
            => ResetByType(stat.ID);

        public void ResetByType(SecondaryStatType type)
        {
            if (_stats.Stats.ContainsKey(type))
                _resetHistory.Stats[type] = _stats.Stats[type];
            _stats.Stats.Remove(type);
        }

        public void ResetByReason(int reason)
        {
            _stats.Stats.Values
                .Where(s => s.Reason == reason)
                .ForEach(s => Reset(s));
        }
    }
}
