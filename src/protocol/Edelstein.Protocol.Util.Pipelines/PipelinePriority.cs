namespace Edelstein.Protocol.Util.Pipelines;

public static class PipelinePriority
{
    public const int Highest = 0x10;
    public const int High = 0x20;
    public const int Normal = 0x30;
    public const int Low = 0x40;
    public const int Lowest = 0x50;

    public const int Default = 0x100;

    public const int After = 0x200;
}
