namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    /// <summary>CUIWorldSelect struct (sizeof=0x1D8, singleton at 0x00C6B1A4)</summary>
    public static readonly Dictionary<int, string> CUIWorldSelect = new()
    {
        [Offsets.CUIWorldSelect.Login] = "CUIWorldSelect.m_pLogin",
        [Offsets.CUIWorldSelect.World] = "CUIWorldSelect.m_nWorld",
        [Offsets.CUIWorldSelect.KeyFocus] = "CUIWorldSelect.m_nKeyFocus",
        [Offsets.CUIWorldSelect.BtWs] = "CUIWorldSelect.m_pBtWS",
        [Offsets.CUIWorldSelect.LayerWorldInfo] = "CUIWorldSelect.m_pLayerWorldInfo",
        [Offsets.CUIWorldSelect.LayerBalloon] = "CUIWorldSelect.m_apLayerBalloon",
        [Offsets.CUIWorldSelect.LayerWorldState] = "CUIWorldSelect.m_apLayerWorldState",
        [Offsets.CUIWorldSelect.BalloonCount] = "CUIWorldSelect.nBalloonCount",
        [Offsets.CUIWorldSelect.LayerAdvice] = "CUIWorldSelect.m_pLayerAdvice",
        [Offsets.CUIWorldSelect.WorldIdx] = "CUIWorldSelect.m_nWorldIdx",
    };
}
