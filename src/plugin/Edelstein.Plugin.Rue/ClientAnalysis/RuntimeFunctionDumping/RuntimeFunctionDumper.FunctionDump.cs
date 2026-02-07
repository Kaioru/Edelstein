using Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public partial class RuntimeFunctionDumper
{
    public record FunctionDump(
        string Name,
        uint BaseAddress,
        uint EndAddress,
        int BytesRead,
        byte[] RawBytes,
        string Disassembly,
        List<CallTarget> Targets,
        List<string> Findings)
    {
        public IReadOnlyList<uint> ExternalCallAddresses =>
         [.. Targets
            .Where(t => t.Kind == TargetKind.ExternalCall)
            .Select(t => t.Address)
            .Distinct()];

        public IReadOnlyList<uint> FollowableAddresses =>
        [.. Targets
            .Where(t => t.Kind is TargetKind.ExternalCall or TargetKind.TailJump)
            .Select(t => t.Address)
            .Distinct()];
    }
}
