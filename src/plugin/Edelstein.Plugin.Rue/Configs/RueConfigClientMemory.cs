namespace Edelstein.Plugin.Rue.Configs;

/// <summary>
/// Configuration for client memory modification.
/// </summary>
public record RueConfigClientMemory
{
    public const int DefaultWatchIntervalMs = 50;
    public const int DefaultWatchDurationMs = 60000;
    public const string WatchModeActive = "active";
    public const string WatchModePassive = "passive";
    public const string DefaultProcessName = "localhost";
    public const int DefaultDumpMaxBytes = 4096;
    public const int DefaultDumpFollowDepth = 1;
    public const int DefaultDumpMaxFunctionsPerLevel = 30;
    public const int DefaultMonitorPollIntervalMs = 50;
    public const int DefaultMonitorTimeoutMs = 10000;

    /// <summary>
    /// Enable client memory modification for auto world/channel selection.
    /// When enabled, sets CWvsContext->m_nWorldID and m_nChannelID directly.
    /// </summary>
    public bool Enabled { get; set; } = false;

    public bool FixupChannelSelectState { get; set; } = true;

    public bool FixupWorldSelectWorldIdx { get; set; } = true;

    public bool WatchEnabled { get; set; } = false;
    public int WatchIntervalMs { get; set; } = DefaultWatchIntervalMs;
    public int WatchDurationMs { get; set; } = DefaultWatchDurationMs;

    /// <summary>
    /// Watch mode for diagnostics:
    /// - "active" (default): Full auto-login with memory modification
    /// - "passive": Just watch memory and log changes during MANUAL login (no auto-login intervention)
    ///
    /// Use "passive" mode to capture a baseline of what the client does during normal manual login,
    /// then compare against "active" mode to identify what's missing.
    /// </summary>
    public string WatchMode { get; set; } = WatchModeActive;

    /// <summary>
    /// Process name to search for (without .exe extension).
    /// </summary>
    public string ProcessName { get; set; } = DefaultProcessName;

    /// <summary>
    /// Enable direct function calling instead of memory writes.
    /// When true, the server calls CLogin::SendLoginPacket via CreateRemoteThread
    /// instead of manually writing to CWvsContext fields.
    /// More invasive but well-tested. Addresses are in V95ClientStructs.Addresses.
    /// </summary>
    public bool UseDirectFunctionCall { get; set; } = false;

    /// <summary>
    /// When true, dumps and disassembles SendLoginPacket and its inner function
    /// at runtime during auto-login. Results saved to plugins/Rue/Reports/.
    /// Enable for reverse engineering sessions to understand what sub_C9E6D4 does.
    /// </summary>
    public bool DumpFunctionsOnLogin { get; set; } = false;

    /// <summary>
    /// Max bytes to read when dumping functions.
    /// SendLoginPacket can be 1600+ bytes post-unpack. Default 4096 captures most functions.
    /// </summary>
    public int DumpMaxBytes { get; set; } = DefaultDumpMaxBytes;

    /// <summary>
    /// How many levels of external calls to follow from the root function.
    /// 0 = only dump the root function.
    /// 1 = root + functions it calls externally.
    /// 2 = root + its calls + their calls.
    /// </summary>
    public int DumpFollowDepth { get; set; } = DefaultDumpFollowDepth;

    /// <summary>
    /// Maximum number of functions to dump per depth level when following call chains.
    /// Known pointers (singletons, registered functions) are prioritized.
    /// Higher values capture more of the call tree but produce larger reports.
    /// </summary>
    public int DumpMaxFunctionsPerLevel { get; set; } = DefaultDumpMaxFunctionsPerLevel;

    /// <summary>
    /// Polling interval in milliseconds for the login step monitor.
    /// The monitor polls client memory (CLogin->m_nLoginStep, singleton pointers)
    /// and signals auto-login steps reactively instead of using fixed delays.
    /// Lower values = faster reaction but slightly more CPU usage.
    /// Only active when Enabled=true and auto-login is running.
    /// </summary>
    public int MonitorPollIntervalMs { get; set; } = DefaultMonitorPollIntervalMs;

    /// <summary>
    /// Maximum time in milliseconds to wait for any single client state condition.
    /// If a monitored condition (e.g., CUIWorldSelect creation) is not met within
    /// this timeout, the auto-login proceeds with a warning (may cause issues).
    /// Set to 0 to disable timeout (not recommended).
    /// </summary>
    public int MonitorTimeoutMs { get; set; } = DefaultMonitorTimeoutMs;
}
