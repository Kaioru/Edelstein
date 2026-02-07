namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    /// <summary>CUIChannelSelect struct (sizeof=0x130, singleton at 0x00C6B1A8)</summary>
    public static readonly Dictionary<int, string> CUIChannelSelect = new()
    {
        [Offsets.CUIChannelSelect.Login] = "CUIChannelSelect.m_pLogin",
        [Offsets.CUIChannelSelect.UserPopulation] = "CUIChannelSelect.m_nUserPopulation",
        [Offsets.CUIChannelSelect.Select] = "CUIChannelSelect.m_nSelect",
        [Offsets.CUIChannelSelect.WorldItem] = "CUIChannelSelect.m_pWorldItem",
        [Offsets.CUIChannelSelect.CanvasWorldName] = "CUIChannelSelect.m_pCanvasWorldName",
        [Offsets.CUIChannelSelect.CanvasGauge] = "CUIChannelSelect.m_pCanvasGauge",
        [Offsets.CUIChannelSelect.LayerSelect] = "CUIChannelSelect.m_pLayerSelect",
        [Offsets.CUIChannelSelect.LayerDummy] = "CUIChannelSelect.m_pLayerDummy",
        [Offsets.CUIChannelSelect.Check] = "CUIChannelSelect.m_bCheck",
        [Offsets.CUIChannelSelect.LayerScroll] = "CUIChannelSelect.m_pLayerScroll",
        [Offsets.CUIChannelSelect.LayerEventDesc] = "CUIChannelSelect.m_pLayerEventDesc",
        [Offsets.CUIChannelSelect.BtCs] = "CUIChannelSelect.m_pBtCS",
        [Offsets.CUIChannelSelect.ConnectionDlg] = "CUIChannelSelect.m_pConnectionDlg",
    };
}
