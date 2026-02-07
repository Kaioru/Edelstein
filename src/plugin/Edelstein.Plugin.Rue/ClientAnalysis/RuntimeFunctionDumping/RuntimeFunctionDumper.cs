using System.Text;
using Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public partial class RuntimeFunctionDumper(IntPtr processHandle, ILogger? logger)
{
    private readonly IntPtr _processHandle = processHandle;
    private readonly ILogger? _logger = logger;

    private static readonly Dictionary<int, string> AllKnownOffsets = V95ClientStructs.GetAllFieldOffsets();
    private readonly Dictionary<uint, string> _knownPointers = [];

    public void RegisterKnownPointer(uint address, string name)
    {
        _knownPointers[address] = name;
    }

    public string AnalyzeFunctionChain(string rootName, uint rootAddress, FunctionChainOptions? options = null)
    {
        var opts = options ?? new FunctionChainOptions();
        var sb = new StringBuilder();

        sb.AppendLine("========================================================================");
        sb.AppendLine("         RUNTIME FUNCTION CHAIN ANALYSIS");
        sb.AppendLine("========================================================================");
        sb.AppendLine();
        sb.AppendLine($"  Timestamp:              {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"  Root function:          {rootName} (0x{rootAddress:X8})");
        sb.AppendLine($"  Max depth:              {opts.MaxDepth}");
        sb.AppendLine($"  Max bytes/function:     {opts.MaxBytesPerFunction}");
        sb.AppendLine($"  Max functions/level:    {opts.MaxFunctionsPerLevel}");
        sb.AppendLine($"  Max instructions:       {opts.MaxInstructions}");
        sb.AppendLine();

        var dumps = new Dictionary<uint, FunctionDump>();
        var dumpOrder = new List<(uint Address, int Depth)>();
        var childrenMap = new Dictionary<uint, List<uint>>();
        var visited = new HashSet<uint>();
        var skippedTargets = new HashSet<uint>();
        var failedReads = new HashSet<uint>();

        List<uint> currentLevel = [rootAddress];

        for (var depth = 0; depth <= opts.MaxDepth && currentLevel.Count > 0; depth++)
        {
            var nextLevelCandidates = new List<uint>();

            foreach (var addr in currentLevel)
            {
                if (!visited.Add(addr))
                    continue;

                var name = addr == rootAddress ? rootName : ResolveTargetName(addr);
                var dump = DumpFunction(name, addr, opts.MaxBytesPerFunction, opts.MaxInstructions);

                if (dump == null)
                {
                    failedReads.Add(addr);
                    _logger?.LogDebug("[FuncDump] Could not read 0x{Addr:X8}", addr);
                    continue;
                }

                dumps[addr] = dump;
                dumpOrder.Add((addr, depth));

                var followable = dump.FollowableAddresses;
                childrenMap[addr] = [.. followable];

                if (depth < opts.MaxDepth)
                    nextLevelCandidates.AddRange(followable.Where(t => !visited.Contains(t)));
            }

            if (depth >= opts.MaxDepth)
                break;

            var candidates = nextLevelCandidates
                .Distinct()
                .Where(a => !visited.Contains(a))
                .ToList();

            currentLevel = PrioritizeTargets(candidates, opts.MaxFunctionsPerLevel);

            var currentLevelSet = new HashSet<uint>(currentLevel);
            foreach (var c in candidates.Where(c => !currentLevelSet.Contains(c)))
                skippedTargets.Add(c);
        }

        var depthGroups = dumpOrder.GroupBy(d => d.Depth).OrderBy(g => g.Key).ToList();
        sb.AppendLine($"  Results: {dumps.Count} functions dumped across {depthGroups.Count} depth level(s)");
        if (skippedTargets.Count > 0)
            sb.AppendLine($"           {skippedTargets.Count} target(s) not followed (level limit)");
        if (failedReads.Count > 0)
            sb.AppendLine($"           {failedReads.Count} target(s) failed to read");
        sb.AppendLine();

        foreach (var depthGroup in depthGroups)
        {
            var depth = depthGroup.Key;
            var funcsAtDepth = depthGroup.ToList();

            sb.AppendLine("========================================================================");
            if (depth == 0)
                sb.AppendLine("  DEPTH 0 — Root");
            else
                sb.AppendLine($"  DEPTH {depth} — {funcsAtDepth.Count} function(s)");
            sb.AppendLine("========================================================================");
            sb.AppendLine();

            foreach (var (addr, _) in funcsAtDepth)
            {
                var dump = dumps[addr];
                AppendSeparator(sb, $"{dump.Name} (0x{addr:X8})");
                AppendDumpSection(sb, dump, includeHexDump: depth > 0, hexDumpMaxBytes: 128);
                sb.AppendLine();
            }
        }

        AppendCallGraph(sb, rootAddress, rootName, childrenMap, dumps, opts.MaxDepth);
        AppendSummary(sb, dumps, dumpOrder);
        AppendKnownOffsetReference(sb);

        return sb.ToString();
    }

    public string AnalyzeSendLoginPacketChain(
        uint sendLoginPacketAddr,
        uint? knownInnerFuncAddr = null,
        int innerMaxBytes = 4096,
        int followDepth = 1)
    {
        if (knownInnerFuncAddr.HasValue && !_knownPointers.ContainsKey(knownInnerFuncAddr.Value))
            RegisterKnownPointer(knownInnerFuncAddr.Value, $"sub_{knownInnerFuncAddr.Value:X} (configured inner)");

        return AnalyzeFunctionChain("CLogin::SendLoginPacket", sendLoginPacketAddr, new FunctionChainOptions
        {
            MaxBytesPerFunction = innerMaxBytes,
            MaxDepth = followDepth,
        });
    }
}
