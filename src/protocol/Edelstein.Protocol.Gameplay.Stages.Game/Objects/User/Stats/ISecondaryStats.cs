using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        IEnumerable<ISecondaryStat> Stats { get; }

        ITwoStateTemporaryStat EnergyChargedStat { get; }
        ITwoStateTemporaryStat DashSpeedStat { get; }
        ITwoStateTemporaryStat DashJumpStat { get; }
        ITwoStateTemporaryStat RideVehicleStat { get; }
        ITwoStatePartyBoosterStat PartyBoosterStat { get; }
        ITwoStateGuidedBulletStat GuidedBulletStat { get; }
        ITwoStateTemporaryStat UndeadStat { get; }

        int GetStatOption(SecondaryStatType type);
        bool HasStatOption(SecondaryStatType type);
    }
}
