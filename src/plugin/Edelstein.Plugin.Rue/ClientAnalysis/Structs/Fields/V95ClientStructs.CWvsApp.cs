namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    /// <summary>CWvsApp struct (sizeof=0x8C, singleton at 0x00C64314)</summary>
    public static readonly Dictionary<int, string> CWvsApp = new()
    {
        [Offsets.CWvsApp.HWnd] = "CWvsApp.m_hWnd",
        [Offsets.CWvsApp.PcomInitialized] = "CWvsApp.m_bPCOMInitialized",
        [Offsets.CWvsApp.MainThreadId] = "CWvsApp.m_dwMainThreadId",
        [Offsets.CWvsApp.HHook] = "CWvsApp.m_hHook",
        [Offsets.CWvsApp.Win9x] = "CWvsApp.m_bWin9x",
        [Offsets.CWvsApp.OsVersion] = "CWvsApp.m_nOSVersion",
        [Offsets.CWvsApp.OsMinorVersion] = "CWvsApp.m_nOSMinorVersion",
        [Offsets.CWvsApp.OsBuildNumber] = "CWvsApp.m_nOSBuildNumber",
        [Offsets.CWvsApp.CsdVersion] = "CWvsApp.m_sCSDVersion",
        [Offsets.CWvsApp.Bit64Info] = "CWvsApp.m_b64BitInfo",
        [Offsets.CWvsApp.UpdateTime] = "CWvsApp.m_tUpdateTime",
        [Offsets.CWvsApp.FirstUpdate] = "CWvsApp.m_bFirstUpdate",
        [Offsets.CWvsApp.CmdLine] = "CWvsApp.m_sCmdLine",
        [Offsets.CWvsApp.GameStartMode] = "CWvsApp.m_nGameStartMode",
        [Offsets.CWvsApp.AutoConnect] = "CWvsApp.m_bAutoConnect",
        [Offsets.CWvsApp.ShowAdBalloon] = "CWvsApp.m_bShowAdBalloon",
        [Offsets.CWvsApp.ExitByTitleEscape] = "CWvsApp.m_bExitByTitleEscape",
        [Offsets.CWvsApp.ZExceptionCode] = "CWvsApp.m_hrZExceptionCode",
        [Offsets.CWvsApp.ComErrorCode] = "CWvsApp.m_hrComErrorCode",
        [Offsets.CWvsApp.SecurityErrorCode] = "CWvsApp.m_dwSecurityErrorCode",
        [Offsets.CWvsApp.TargetVersion] = "CWvsApp.m_nTargetVersion",
        [Offsets.CWvsApp.LastServerIpCheck] = "CWvsApp.m_tLastServerIPCheck",
        [Offsets.CWvsApp.EnabledDx9] = "CWvsApp.m_bEnabledDX9",
        [Offsets.CWvsApp.WindowActive] = "CWvsApp.m_bWindowActive",
    };
}
