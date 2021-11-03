using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class SecondaryStats : ISecondaryStats
    {
        public IDictionary<SecondaryStatType, ITemporaryStat> Stats { get; }

        public SecondaryStats()
        {
            // TODO: TwoState 
        }

        public bool HasStat(SecondaryStatType type)
            => Stats.ContainsKey(type);

        public int GetValue(SecondaryStatType type)
            => HasStat(type) ? Stats[type].Value : 0;

        public int GetReason(SecondaryStatType type)
            => HasStat(type) ? Stats[type].Reason : 0;

        public ITemporaryStat GetStat(SecondaryStatType type)
            => HasStat(type) ? Stats[type] : null;

        public IDictionary<SecondaryStatType, ITemporaryStat> ToDictionary()
            => Stats.ToImmutableDictionary();
    }
}
