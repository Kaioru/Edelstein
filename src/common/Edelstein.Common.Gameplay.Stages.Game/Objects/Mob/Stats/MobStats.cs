using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public class MobStats : IMobStats
    {
        public IDictionary<MobStatType, IMobStat> Stats { get; }

        public MobStats()
        {
            Stats = new Dictionary<MobStatType, IMobStat>();
            // TODO: BurnedInfo.. etc 
        }

        public bool HasStat(MobStatType type)
            => Stats.ContainsKey(type);

        public int GetValue(MobStatType type)
            => HasStat(type) ? Stats[type].Value : 0;

        public int GetReason(MobStatType type)
            => HasStat(type) ? Stats[type].Reason : 0;

        public IMobStat GetStat(MobStatType type)
            => HasStat(type) ? Stats[type] : null;

        public IDictionary<MobStatType, IMobStat> ToDictionary()
            => Stats.ToImmutableDictionary();
    }
}
