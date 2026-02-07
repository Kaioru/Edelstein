namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    /// <summary>
    /// Returns a merged dictionary of ALL known struct offsets for annotation.
    /// Used by RuntimeFunctionDumper to label field accesses in disassembly.
    /// </summary>
    public static Dictionary<int, string> GetAllFieldOffsets()
    {
        var all = new Dictionary<int, string>();
        MergeInto(all, CLogin);
        MergeInto(all, CWvsContext);
        MergeInto(all, CUIWorldSelect);
        MergeInto(all, CUIChannelSelect);
        MergeInto(all, CQuestMan);
        MergeInto(all, CWvsApp);
        MergeInto(all, WorldItem);
        MergeInto(all, ChannelItem);
        return all;
    }

    private static void MergeInto(Dictionary<int, string> target, Dictionary<int, string> source)
    {
        foreach (var kv in source)
        {
            if (target.TryGetValue(kv.Key, out var existing))
                target[kv.Key] = $"{existing} / {kv.Value}";
            else
                target[kv.Key] = kv.Value;
        }
    }

}
