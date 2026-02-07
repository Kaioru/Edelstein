namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    /// <summary>
    /// Known singleton pointer addresses in the .data section.
    /// These are POINTERS to instance objects, not the objects themselves.
    /// </summary>
    public static readonly Dictionary<uint, string> Singletons = new()
    {
        [Addresses.CUniqueModelessSingletonPtr] = "TSingleton<CUniqueModeless>::ms_pInstance",
        [Addresses.CClientSocketSingletonPtr] = "TSingleton<CClientSocket>::ms_pInstance",
        [Addresses.CWvsContextSingletonPtr] = "TSingleton<CWvsContext>::ms_pInstance",
        [Addresses.CWvsAppSingletonPtr] = "TSingleton<CWvsApp>::ms_pInstance",
        [Addresses.CUITitleSingletonPtr] = "TSingleton<CUITitle>::ms_pInstance",
        [Addresses.CLicenseDlgSingletonPtr] = "TSingleton<CLicenseDlg>::ms_pInstance",
        [Addresses.CConnectionNoticeDlgSingletonPtr] = "TSingleton<CConnectionNoticeDlg>::ms_pInstance",
        [Addresses.CUIWorldSelectSingletonPtr] = "TSingleton<CUIWorldSelect>::ms_pInstance",
        [Addresses.CUIChannelSelectSingletonPtr] = "TSingleton<CUIChannelSelect>::ms_pInstance",
        [Addresses.CUIRecommendWorldSingletonPtr] = "TSingleton<CUIRecommendWorld>::ms_pInstance",
        [Addresses.CUICharSelectSingletonPtr] = "TSingleton<CUICharSelect>::ms_pInstance",
        [Addresses.CUICharDetailSingletonPtr] = "TSingleton<CUICharDetail>::ms_pInstance",
        [Addresses.CUIAvatarSingletonPtr] = "TSingleton<CUIAvatar>::ms_pInstance",
        [Addresses.CUICharDetailVacSingletonPtr] = "TSingleton<CUICharDetailVAC>::ms_pInstance",
        [Addresses.CUIAvatarVacSingletonPtr] = "TSingleton<CUIAvatarVAC>::ms_pInstance",
        [Addresses.CUINewCharRaceSelectSingletonPtr] = "TSingleton<CUINewCharRaceSelect>::ms_pInstance",
        [Addresses.CUINewCharNameSelectCygnusSingletonPtr] = "TSingleton<CUINewCharNameSelectCygnus>::ms_pInstance",
        [Addresses.CUINewCharNameSelectNormalSingletonPtr] = "TSingleton<CUINewCharNameSelectNormal>::ms_pInstance",
        [Addresses.CUINewCharNameSelectAranSingletonPtr] = "TSingleton<CUINewCharNameSelectAran>::ms_pInstance",
        [Addresses.CUINewCharNameSelectEvanSingletonPtr] = "TSingleton<CUINewCharNameSelectEvan>::ms_pInstance",
        [Addresses.CUINewCharJobSelectSingletonPtr] = "TSingleton<CUINewCharJobSelect>::ms_pInstance",
        [Addresses.CUINewCharAvatarSelectSingletonPtr] = "TSingleton<CUINewCharAvatarSelect>::ms_pInstance",
        [Addresses.CLoginGradeWndSingletonPtr] = "TSingleton<CLoginGradeWnd>::ms_pInstance",
        [Addresses.CUIGetUserInfoSingletonPtr] = "TSingleton<CUIGetUserInfo>::ms_pInstance",
        [Addresses.CNmcoClientObjectSingletonPtr] = "CNMCOClientObject::spInstance",
        [Addresses.CQuestManSingletonPtr] = "TSingleton<CQuestMan>::ms_pInstance",
        [Addresses.CClientSocketSendPacketPtr] = "CClientSocket (SendPacket xref)",
    };
}
