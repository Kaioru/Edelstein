namespace Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

/// <summary>Options for recursive function chain analysis.</summary>
public record FunctionChainOptions
{
    /// <summary>Maximum bytes to read per function (buffer size). Default 4096.</summary>
    public int MaxBytesPerFunction { get; init; } = 4096;

    /// <summary>
    /// How many levels of external calls to follow from the root.
    /// 0 = only dump the root function.
    /// 1 = root + functions it calls externally.
    /// 2 = root + its calls + their calls.
    /// Default 1.
    /// </summary>
    public int MaxDepth { get; init; } = 1;

    /// <summary>Maximum functions to dump per depth level. Default 30.</summary>
    public int MaxFunctionsPerLevel { get; init; } = 30;

    /// <summary>Maximum instructions to decode per function. Default 500.</summary>
    public int MaxInstructions { get; init; } = 500;
}
