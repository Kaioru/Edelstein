namespace Edelstein.Protocol.Utilities.Pipelines;

public static class PipelinePriority
{
    public const int Highest = 0x10;
    public const int High = 0x20;
    public const int Normal = 0x30;
    public const int Low = 0x40;
    public const int Lowest = 0x50;

    public const int Reserved = 0x100;

    public const int PostHighest = 0x150;
    public const int PostHigh = 0x160;
    public const int PostNormal = 0x170;
    public const int PostLow = 0x180;
    public const int PostLowest = 0x190;
}
