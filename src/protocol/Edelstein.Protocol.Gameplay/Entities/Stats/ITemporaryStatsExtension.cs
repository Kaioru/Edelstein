using Edelstein.Protocol.Utilities;

namespace Edelstein.Protocol.Gameplay.Entities.Stats;

public static class ITemporaryStatsExtension
{
    public static Flags GetFlags(this ITemporaryStats stats)
    {
        var flag = new Flags(128);

        foreach (var type in stats.Records.Keys)
            flag.SetFlag((int)type);

        if (stats.DiceInfo != null)
            flag.SetFlag((int)TemporaryStatType.Dice);

        if (stats.EnergyCharged != null)
            flag.SetFlag((int)TemporaryStatType.EnergyCharged);
        if (stats.DashSpeed != null)
            flag.SetFlag((int)TemporaryStatType.Dash_Speed);
        if (stats.DashJump != null)
            flag.SetFlag((int)TemporaryStatType.Dash_Jump);
        if (stats.RideVehicle != null)
            flag.SetFlag((int)TemporaryStatType.RideVehicle);
        if (stats.PartyBooster != null)
            flag.SetFlag((int)TemporaryStatType.PartyBooster);
        if (stats.GuidedBullet != null)
            flag.SetFlag((int)TemporaryStatType.GuidedBullet);
        if (stats.Undead != null)
            flag.SetFlag((int)TemporaryStatType.Undead);

        return flag;
    }
    
    public static StructuredTemporaryStatsLocal ToStructuredLocal(this ITemporaryStats stats)
        => new()
        {
            Stats = stats
        };
    
    public static StructuredTemporaryStatsRemote ToStructuredRemote(this ITemporaryStats stats)
        => new()
        {
            Stats = stats
        };
}
