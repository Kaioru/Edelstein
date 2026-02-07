namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static class TypeSizes
{
    /// <summary>
    /// This is useful because x86 client pointers are 4 bytes = sizeof(int)
    /// </summary>
    public const int Int32 = sizeof(int);
}
