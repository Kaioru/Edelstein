using System;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats.Modify;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats.Modify
{
    public class ModifyMobStatContext : IModifyMobStatContext
    {
        private readonly MobStats _stats;

        private readonly MobStats _setHistory;
        private readonly MobStats _resetHistory;

        public IMobStats SetHistory => _setHistory;
        public IMobStats ResetHistory => _resetHistory;

        public ModifyMobStatContext(MobStats stats)
        {
            _stats = stats;

            _setHistory = new MobStats();
            _resetHistory = new MobStats();
        }

        public void Set(IMobStat stat)
        {
            Reset(stat);

            _setHistory.Stats[stat.Type] = stat;
            _stats.Stats[stat.Type] = stat;
        }

        public void Set(
            MobStatType type,
            int value,
            int reason,
            DateTime? dateExpire = null
        )
            => Set(new MobStat
            {
                Type = type,
                Value = value,
                Reason = reason,
                DateExpire = dateExpire
            });

        public void Reset(IMobStat stat)
            => ResetByType(stat.Type);

        public void ResetByType(MobStatType type)
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