namespace Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

public static class StructFieldRegistry
{
    private static readonly Dictionary<string, Dictionary<int, string>> Fields = new(StringComparer.Ordinal)
    {
        ["CLogin"] = V95ClientStructs.CLogin,
        ["CWvsContext"] = V95ClientStructs.CWvsContext,
        ["CUIWorldSelect"] = V95ClientStructs.CUIWorldSelect,
        ["CUIChannelSelect"] = V95ClientStructs.CUIChannelSelect,
        ["CQuestMan"] = V95ClientStructs.CQuestMan,
        ["CWvsApp"] = V95ClientStructs.CWvsApp,
        ["WorldItem"] = V95ClientStructs.WorldItem,
        ["ChannelItem"] = V95ClientStructs.ChannelItem
    };

    private static readonly Dictionary<string, string> PointerFields = new(StringComparer.Ordinal)
    {
        [$"CUIWorldSelect:{V95ClientStructs.Offsets.CUIWorldSelect.Login}"] = "CLogin",
        [$"CUIChannelSelect:{V95ClientStructs.Offsets.CUIChannelSelect.Login}"] = "CLogin",
        [$"CUIChannelSelect:{V95ClientStructs.Offsets.CUIChannelSelect.WorldItem}"] = "WorldItem",
        [$"WorldItem:{V95ClientStructs.Offsets.WorldItem.ChannelItemsPtr}"] = "ChannelItem"
    };

    public static bool TryGetField(string typeName, int offset, out string fieldName)
    {
        if (Fields.TryGetValue(typeName, out var map) && map.TryGetValue(offset, out var name))
        {
            fieldName = name;
            return true;
        }

        fieldName = string.Empty;
        return false;
    }

    public static bool ContainsType(string typeName) => Fields.ContainsKey(typeName);

    public static bool TryGetPointerTarget(string typeName, int offset, out string targetType)
    {
        if (PointerFields.TryGetValue($"{typeName}:{offset}", out var resolved))
        {
            targetType = resolved;
            return true;
        }

        targetType = string.Empty;
        return false;
    }
}
