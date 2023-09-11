using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public static class CharacterTemporaryStatsExtensions
{
    public static bool HasTwoStateStats(this ICharacterTemporaryStats stats)
        => stats.EnergyChargedRecord != null ||
           stats.DashSpeedRecord != null ||
           stats.DashJumpRecord != null ||
           stats.RideVehicleRecord != null ||
           stats.PartyBoosterRecord != null ||
           stats.GuidedBulletRecord != null ||
           stats.UndeadRecord != null;
}
