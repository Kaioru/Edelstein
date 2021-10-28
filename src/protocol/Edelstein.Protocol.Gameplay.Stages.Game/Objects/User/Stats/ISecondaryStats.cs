using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        int this[SecondaryStatType type] { get; }

        ITwoStateTemporaryStat EnergyCharged { get; }
        ITwoStateTemporaryStat DashSpeed { get; }
        ITwoStateTemporaryStat DashJump { get; }
        ITwoStateTemporaryStat RideVehicle { get; }
        ITwoStatePartyBoosterStat PartyBooster { get; }
        ITwoStateGuidedBulletStat GuidedBullet { get; }
        ITwoStateTemporaryStat Undead { get; }

        ICollection<ITemporaryStat> All();
    }
}
