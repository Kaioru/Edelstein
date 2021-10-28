using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class SecondaryStats : ISecondaryStats
    {
        public IDictionary<SecondaryStatType, ITemporaryStat> Stats { get; }

        public int this[SecondaryStatType type] => Stats.ContainsKey(type) ? Stats[type].Value : 0;

        public SecondaryStats()
        {
            Stats = new Dictionary<SecondaryStatType, ITemporaryStat>();

            // TODO: TwoState 
        }

        public IDictionary<SecondaryStatType, ITemporaryStat> All() => Stats.ToImmutableDictionary();
    }
}
