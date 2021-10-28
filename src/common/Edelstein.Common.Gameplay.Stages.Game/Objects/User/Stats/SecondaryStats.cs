using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class SecondaryStats : ISecondaryStats
    {
        private readonly IDictionary<SecondaryStatType, ITemporaryStat> _stats;

        public int this[SecondaryStatType type] => _stats.ContainsKey(type) ? _stats[type].Value : 0;

        public ITwoStateTemporaryStat EnergyCharged { get; set; }
        public ITwoStateTemporaryStat DashSpeed { get; set; }
        public ITwoStateTemporaryStat DashJump { get; set; }
        public ITwoStateTemporaryStat RideVehicle { get; set; }
        public ITwoStatePartyBoosterStat PartyBooster { get; set; }
        public ITwoStateGuidedBulletStat GuidedBullet { get; set; }
        public ITwoStateTemporaryStat Undead { get; set; }

        public SecondaryStats()
        {
            _stats = new Dictionary<SecondaryStatType, ITemporaryStat>();
        }

        public ICollection<ITemporaryStat> All()
        {
            var stats = _stats.Values;

            if (EnergyCharged != null) stats.Add(EnergyCharged);
            if (DashSpeed != null) stats.Add(DashSpeed);
            if (DashJump != null) stats.Add(DashJump);
            if (RideVehicle != null) stats.Add(RideVehicle);
            if (PartyBooster != null) stats.Add(PartyBooster);
            if (GuidedBullet != null) stats.Add(GuidedBullet);
            if (Undead != null) stats.Add(Undead);

            return stats;
        }
    }
}
