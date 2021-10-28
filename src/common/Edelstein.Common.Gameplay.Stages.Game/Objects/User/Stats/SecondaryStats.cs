using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class SecondaryStats : ISecondaryStats
    {
        private readonly IDictionary<SecondaryStatType, ITemporaryStat> _stats;

        public int this[SecondaryStatType type] => _stats.ContainsKey(type) ? _stats[type].Value : 0;

        public ITwoStateTemporaryStat EnergyCharged { get; }
        public ITwoStateTemporaryStat DashSpeed { get; }
        public ITwoStateTemporaryStat DashJump { get; }
        public ITwoStateTemporaryStat RideVehicle { get; }
        public ITwoStatePartyBoosterStat PartyBooster { get; }
        public ITwoStateGuidedBulletStat GuidedBullet { get; }
        public ITwoStateTemporaryStat Undead { get; }

        public SecondaryStats()
        {
            _stats = new Dictionary<SecondaryStatType, ITemporaryStat>();

            // TODO: TwoState initializers
        }

        public IDictionary<SecondaryStatType, ITemporaryStat> All() => _stats.ToImmutableDictionary();
    }
}
