namespace Edelstein.Plugin.Rue.ClientAnalysis;

public partial class RuntimeFunctionDumper
{
    private string ResolveTargetName(uint address)
    {
        return _knownPointers.TryGetValue(address, out var name) ? name : $"sub_{address:X}";
    }

    private List<uint> PrioritizeTargets(List<uint> candidates, int maxCount)
    {
        if (candidates.Count <= maxCount)
            return candidates;

        return [.. candidates
            .OrderByDescending(_knownPointers.ContainsKey)
            .ThenBy(addr => addr)
            .Take(maxCount)];
    }
}
