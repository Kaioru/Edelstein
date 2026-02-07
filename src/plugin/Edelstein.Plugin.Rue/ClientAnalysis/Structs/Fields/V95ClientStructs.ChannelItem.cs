namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    /// <summary>CHANNELITEM struct (sizeof=0x14, array element within WORLDITEM)</summary>
    public static readonly Dictionary<int, string> ChannelItem = new()
    {
        [Offsets.ChannelItem.NamePtr] = "CHANNELITEM.sName",
        [Offsets.ChannelItem.WorldId] = "CHANNELITEM.nWorldID",
        [Offsets.ChannelItem.ChannelId] = "CHANNELITEM.nChannelID",
        [Offsets.ChannelItem.AdultFlag] = "CHANNELITEM.bAdult",
    };
}
