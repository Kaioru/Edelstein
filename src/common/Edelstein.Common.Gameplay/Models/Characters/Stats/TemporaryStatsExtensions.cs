using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats;

public static class TemporaryStatsExtensions
{
    public static bool HasTwoStateStats(this ITemporaryStats stats)
        => stats.EnergyChargedRecord != null ||
           stats.DashSpeedRecord != null ||
           stats.DashJumpRecord != null ||
           stats.RideVehicleRecord != null ||
           stats.PartyBoosterRecord != null ||
           stats.GuidedBulletRecord != null ||
           stats.UndeadRecord != null;
}
