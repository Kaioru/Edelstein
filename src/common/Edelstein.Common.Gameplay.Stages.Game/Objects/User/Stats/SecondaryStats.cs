using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class SecondaryStats : ISecondaryStats
    {
        public int this[SecondaryStatType type] => Stats.ContainsKey(type) ? Stats[type].Value : 0;
        public IDictionary<SecondaryStatType, ITemporaryStat> Stats { get; }

        public SecondaryStats()
        {
            // TODO: TwoState 
        }
    }
}
