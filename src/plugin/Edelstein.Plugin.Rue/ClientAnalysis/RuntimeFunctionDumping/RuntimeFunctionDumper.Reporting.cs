using System.Text;
using Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;
using Edelstein.Plugin.Rue.Diagnostics;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public partial class RuntimeFunctionDumper
{
    private static void AppendSeparator(StringBuilder sb, string title)
    {
        sb.AppendLine("------------------------------------------------------------------------");
        sb.AppendLine($"  {title}");
        sb.AppendLine("------------------------------------------------------------------------");
    }

    private void AppendDumpSection(StringBuilder sb, FunctionDump dump, bool includeHexDump, int hexDumpMaxBytes = 64)
    {
        var decodedBytes = dump.EndAddress - dump.BaseAddress;
        var extCalls = dump.Targets.Count(t => t.Kind == TargetKind.ExternalCall);
        var extCallsDistinct = dump.ExternalCallAddresses.Count;
        var intBranches = dump.Targets.Count(t => t.Kind == TargetKind.InternalBranch);
        var tailJumps = dump.Targets.Count(t => t.Kind == TargetKind.TailJump);

        sb.AppendLine($"  Range: 0x{dump.BaseAddress:X8} - 0x{dump.EndAddress:X8}  |  {decodedBytes} bytes decoded  |  {dump.BytesRead} bytes read");
        sb.AppendLine($"  Targets: {extCallsDistinct} external calls, {intBranches} internal branches, {tailJumps} tail jumps");
        sb.AppendLine();

        if (includeHexDump)
        {
            sb.AppendLine("  Raw bytes:");
            HexFormatter.AppendHexDump(sb, dump.RawBytes, dump.BaseAddress, hexDumpMaxBytes);
            sb.AppendLine();
        }

        sb.AppendLine("  Disassembly:");
        sb.AppendLine(dump.Disassembly);

        var followable = dump.FollowableAddresses;
        if (followable.Count > 0)
        {
            sb.AppendLine("  External call targets:");
            foreach (var target in followable)
            {
                var name = ResolveTargetName(target);
                sb.AppendLine($"    -> 0x{target:X8}  {name}");
            }
            sb.AppendLine();
        }

        if (dump.Findings.Count > 0)
        {
            sb.AppendLine("  Findings:");
            foreach (var f in dump.Findings)
                sb.AppendLine($"    - {f}");
            sb.AppendLine();
        }
    }

    private void AppendCallGraph(
        StringBuilder sb, uint rootAddress, string rootName,
        Dictionary<uint, List<uint>> childrenMap, Dictionary<uint, FunctionDump> dumps,
        int maxDepth)
    {
        sb.AppendLine("========================================================================");
        sb.AppendLine("  CALL GRAPH");
        sb.AppendLine("========================================================================");
        sb.AppendLine();

        var dumpedSet = new HashSet<uint>(dumps.Keys);
        sb.AppendLine($"  {rootName} (0x{rootAddress:X8})");

        if (childrenMap.TryGetValue(rootAddress, out var rootKids) && rootKids.Count > 0)
        {
            var treeVisited = new HashSet<uint> { rootAddress };
            for (var i = 0; i < rootKids.Count; i++)
            {
                AppendCallGraphNode(sb, rootKids[i], "  ", i == rootKids.Count - 1,
                    childrenMap, dumpedSet, treeVisited, 1, maxDepth);
            }
        }

        sb.AppendLine();
    }

    private void AppendCallGraphNode(
        StringBuilder sb, uint addr, string indent, bool isLast,
        Dictionary<uint, List<uint>> childrenMap, HashSet<uint> dumpedSet,
        HashSet<uint> visited, int depth, int maxDepth)
    {
        var connector = isLast ? "\\-- " : "|-- ";
        var name = ResolveTargetName(addr);

        if (!visited.Add(addr))
        {
            sb.AppendLine($"{indent}{connector}0x{addr:X8}  {name}  [cycle]");
            return;
        }

        var status = dumpedSet.Contains(addr) ? "" : "  [not followed]";
        sb.AppendLine($"{indent}{connector}0x{addr:X8}  {name}{status}");

        if (childrenMap.TryGetValue(addr, out var kids) && kids.Count > 0 && depth <= maxDepth)
        {
            var childIndent = indent + (isLast ? "    " : "|   ");
            for (var i = 0; i < kids.Count; i++)
            {
                AppendCallGraphNode(sb, kids[i], childIndent, i == kids.Count - 1,
                    childrenMap, dumpedSet, visited, depth + 1, maxDepth);
            }
        }
    }

    private static void AppendSummary(
        StringBuilder sb,
        Dictionary<uint, FunctionDump> dumps,
        List<(uint Address, int Depth)> dumpOrder)
    {
        sb.AppendLine("========================================================================");
        sb.AppendLine("  SUMMARY");
        sb.AppendLine("========================================================================");
        sb.AppendLine();

        var allFindings = new List<(string FuncName, string Finding)>();
        foreach (var (addr, _) in dumpOrder)
        {
            var dump = dumps[addr];
            foreach (var finding in dump.Findings)
                allFindings.Add((dump.Name, finding));
        }

        var writes = allFindings.Where(f => f.Finding.Contains("[WRITE]")).ToList();
        var reads = allFindings.Where(f => f.Finding.Contains("[READ]")).ToList();

        if (writes.Count > 0)
        {
            sb.AppendLine("  Memory WRITES to known fields:");
            foreach (var (funcName, finding) in writes)
                sb.AppendLine($"    {finding}  (in {funcName})");
            sb.AppendLine();
        }

        if (reads.Count > 0)
        {
            sb.AppendLine("  Memory READS from known fields:");
            foreach (var (funcName, finding) in reads)
                sb.AppendLine($"    {finding}  (in {funcName})");
            sb.AppendLine();
        }

        if (writes.Count == 0 && reads.Count == 0)
        {
            sb.AppendLine("  No accesses to known CWvsContext/CLogin offsets detected.");
            sb.AppendLine("  This may mean:");
            sb.AppendLine("    - The function is VM-protected (bytecode, not native x86)");
            sb.AppendLine("    - The offsets are accessed indirectly (via computed addresses)");
            sb.AppendLine("    - The function does something unexpected");
            sb.AppendLine();
        }

        var vmIndicators = allFindings.Where(f =>
            f.Finding.Contains("VM-protected") || f.Finding.Contains("INVALID RATIO") || f.Finding.Contains("trampoline")).ToList();
        if (vmIndicators.Count > 0)
        {
            sb.AppendLine("  VM PROTECTION INDICATORS:");
            foreach (var (funcName, finding) in vmIndicators)
                sb.AppendLine($"    ! {finding}  (in {funcName})");
            sb.AppendLine();
            sb.AppendLine("  If code is VM-protected, alternative approaches:");
            sb.AppendLine("    1. Hook the function entry/exit to trace register/memory state");
            sb.AppendLine("    2. Set hardware breakpoints on CWvsContext offsets during manual login");
            sb.AppendLine("    3. Use a Themida devirtualizer tool");
            sb.AppendLine("    4. Trace memory writes by comparing snapshots before/after DFC call");
            sb.AppendLine();
        }
    }

    private static void AppendKnownOffsetReference(StringBuilder sb)
    {
        sb.AppendLine("------------------------------------------------------------------------");
        sb.AppendLine("  KNOWN OFFSET REFERENCE");
        sb.AppendLine("------------------------------------------------------------------------");
        sb.AppendLine();

        var structDicts = new (string Name, Dictionary<int, string> Fields)[]
        {
            ("CLogin", V95ClientStructs.CLogin),
            ("CWvsContext", V95ClientStructs.CWvsContext),
            ("CUIWorldSelect", V95ClientStructs.CUIWorldSelect),
            ("CUIChannelSelect", V95ClientStructs.CUIChannelSelect),
            ("CQuestMan", V95ClientStructs.CQuestMan),
            ("CWvsApp", V95ClientStructs.CWvsApp),
        };

        foreach (var (name, fields) in structDicts)
        {
            sb.AppendLine($"  {name} ({fields.Count} fields):");
            foreach (var kv in fields.OrderBy(f => f.Key))
                sb.AppendLine($"    +0x{kv.Key:X4} = {kv.Value}");
            sb.AppendLine();
        }
    }
}
