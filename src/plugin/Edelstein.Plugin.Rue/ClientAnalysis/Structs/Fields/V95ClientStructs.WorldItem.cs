namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    /// <summary>WORLDITEM struct (sizeof=0x20, array element of CLogin::m_WorldItem)</summary>
    public static readonly Dictionary<int, string> WorldItem = new()
    {
        [Offsets.WorldItem.Id] = "WORLDITEM.nWorldID",
        [Offsets.WorldItem.NamePtr] = "WORLDITEM.sName",
        [Offsets.WorldItem.ChannelItemsPtr] = "WORLDITEM.aChannelItem",
    };
}
