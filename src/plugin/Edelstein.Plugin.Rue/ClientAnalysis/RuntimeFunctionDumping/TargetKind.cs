namespace Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

/// <summary>Classification of a branch/call target relative to the containing function.</summary>
public enum TargetKind : byte
{
    /// <summary>CALL instruction targeting an address outside the function's decoded range.</summary>
    ExternalCall,

    /// <summary>Unconditional JMP targeting an address outside the function's decoded range (likely a tail call).</summary>
    TailJump,

    /// <summary>Branch (JMP/Jcc) within the function's decoded range (internal control flow).</summary>
    InternalBranch,
}
