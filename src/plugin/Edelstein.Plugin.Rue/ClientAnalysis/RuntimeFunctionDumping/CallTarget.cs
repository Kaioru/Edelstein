namespace Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

/// <summary>A resolved branch/call target with source location and classification.</summary>
public record CallTarget(uint Address, uint SourceIP, TargetKind Kind);
