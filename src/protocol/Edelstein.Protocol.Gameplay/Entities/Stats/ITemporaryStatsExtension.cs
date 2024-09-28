namespace Edelstein.Protocol.Gameplay.Entities.Stats;

public static class ITemporaryStatsExtension
{
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
