using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;
using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats : IPacketWritable
    {
        ITwoStateTemporaryStat EnergyCharged { get; }
        ITwoStateTemporaryStat DashSpeed { get; }
        ITwoStateTemporaryStat DashJump { get; }
        ITwoStateTemporaryStat RideVehicle { get; }
        ITwoStatePartyBoosterStat PartyBooster { get; }
        ITwoStateGuidedBulletStat GuidedBullet { get; }
        ITwoStateTemporaryStat Undead { get; }

        bool HasStatOption(SecondaryStatType type);
        int GetStatOption(SecondaryStatType type);
    }
}
