using System.Collections.Concurrent;
using System.Text;

namespace Edelstein.Plugin.Rue.Diagnostics;

/// <summary>
/// Diagnostics for the v95 client auto-login flow.
/// Tracks server packets, client memory writes, and login step transitions.
///
/// ## Auto-Login Flow:
///  1. Server sends CheckPasswordResult → client state: none → CheckPassword
///  2. Monitor starts polling client memory → CLoginGradeWnd appears
///  3. Server state: CheckPassword → SelectWorld
///  4. CUIWorldSelect appears → server sends CheckUserLimitResult
///  5. Client step: unknown → SelectWorld → CUIChannelSelect appears
///  6. Write CWvsContext world/channel + mirror CQuestMan.m_nWorldID
///  7. Fixup CWvsContext channel arrays from world item data
///  8. Set CLogin.m_bRequestSent = 1
///  9. Server sends SelectWorldResult → client state: SelectWorld → SelectCharacter
/// 10. Client step: SelectWorld → SelectCharacter → character selected
/// 11. Login complete
///
/// ## Timing Gates:
/// - CUIWorldSelect MUST exist before sending CheckUserLimitResult
/// - CUIChannelSelect MUST exist before writing CWvsContext values
/// - CLogin step MUST be stable (not transitioning) before setting m_bRequestSent
/// - CWvsContext world/channel MUST be written before SelectWorldResult
/// </summary>
public class LoginDiagnostics
{
    private readonly ConcurrentQueue<DiagnosticEvent> _events = [];
    private readonly object _snapshotLock = new();
    private MemorySnapshot? _lastSnapshot;
    private MemorySnapshot? _crashSnapshot;
    private static LoginTimeline? _savedManualTimeline;
    private static LoginTimeline? _savedAutoTimeline;

    internal DateTime StartTime { get; private set; } = DateTime.UtcNow;
    internal ConcurrentDictionary<string, DateTime> FlowMilestones { get; } = [];

    public void Reset()
    {
        while (_events.TryDequeue(out _)) { }
        lock (_snapshotLock)
        {
            _lastSnapshot = null;
            _crashSnapshot = null;
        }
        FlowMilestones.Clear();
        StartTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Saves the current snapshot as the "state at crash time".
    /// Call this when a crash is detected BEFORE the client cleans up.
    /// </summary>
    public void SaveCrashSnapshot()
    {
        lock (_snapshotLock)
        {
            _crashSnapshot = _lastSnapshot;
        }
    }

    /// <summary>
    /// Gets the crash snapshot (state at time of crash, not after cleanup).
    /// </summary>
    public MemorySnapshot? GetCrashSnapshot()
    {
        lock (_snapshotLock)
        {
            return _crashSnapshot;
        }
    }

    /// <summary>
    /// Records a flow milestone for timing analysis.
    /// </summary>
    public void RecordMilestone(string name)
    {
        FlowMilestones.TryAdd(name, DateTime.UtcNow);
    }

    /// <summary>
    /// Gets the time between two milestones in milliseconds.
    /// </summary>
    public double? GetMilestoneInterval(string from, string to)
    {
        if (FlowMilestones.TryGetValue(from, out var fromTime) &&
            FlowMilestones.TryGetValue(to, out var toTime))
        {
            return (toTime - fromTime).TotalMilliseconds;
        }
        return null;
    }

    /// <summary>
    /// Saves the current timeline as a manual login baseline.
    /// </summary>
    public void SaveAsManualBaseline()
    {
        _savedManualTimeline = LoginTimeline.Capture(this, "manual");
    }

    /// <summary>
    /// Saves the current timeline as an auto login attempt.
    /// </summary>
    public void SaveAsAutoAttempt()
    {
        _savedAutoTimeline = LoginTimeline.Capture(this, "auto");
    }

    /// <summary>
    /// Checks if we have both timelines for comparison.
    /// </summary>
    public static bool HasBothTimelines => _savedManualTimeline != null && _savedAutoTimeline != null;

    /// <summary>
    /// Directory for saving diagnostic reports.
    /// </summary>
    public static string ReportsDirectory { get; set; } = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "plugins", "Rue", "Reports"
    );

    /// <summary>
    /// Saves a report to a file with timestamp.
    /// </summary>
    public static string SaveReportToFile(string report, string prefix)
    {
        try
        {
            Directory.CreateDirectory(ReportsDirectory);
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var filename = $"{prefix}_{timestamp}.txt";
            var filepath = Path.Combine(ReportsDirectory, filename);
            File.WriteAllText(filepath, report);
            return filepath;
        }
        catch (Exception ex)
        {
            return $"Failed to save: {ex.Message}";
        }
    }

    /// <summary>
    /// Saves the current timeline report to a file.
    /// </summary>
    public string SaveReport(string mode)
    {
        var report = GenerateReport();
        return SaveReportToFile(report, $"login_{mode}");
    }

    /// <summary>
    /// Saves the comparison report to a file.
    /// </summary>
    public static string SaveComparisonReport()
    {
        var report = GenerateComparison();
        return SaveReportToFile(report, "comparison");
    }

    /// <summary>
    /// Generates a side-by-side comparison of manual vs auto login timelines.
    /// </summary>
    public static string GenerateComparison()
    {
        var sb = new StringBuilder();
        sb.AppendLine("╔══════════════════════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║              MANUAL vs AUTO LOGIN COMPARISON                                         ║");
        sb.AppendLine("╚══════════════════════════════════════════════════════════════════════════════════════╝");
        sb.AppendLine();

        if (_savedManualTimeline == null)
        {
            sb.AppendLine("⚠ No manual login baseline captured yet.");
            sb.AppendLine("  Run with WatchMode='passive', perform manual login, then the timeline is auto-saved.");
            sb.AppendLine();
        }

        if (_savedAutoTimeline == null)
        {
            sb.AppendLine("⚠ No auto login attempt captured yet.");
            sb.AppendLine("  Run with WatchMode='active' to capture an auto-login attempt.");
            sb.AppendLine();
        }

        if (_savedManualTimeline == null || _savedAutoTimeline == null)
        {
            return sb.ToString();
        }

        // Header info
        sb.AppendLine($"Manual Login: {_savedManualTimeline.CaptureTime:HH:mm:ss} ({_savedManualTimeline.TotalDuration.TotalSeconds:F1}s, {_savedManualTimeline.Events.Length} events)");
        sb.AppendLine($"Auto Login:   {_savedAutoTimeline.CaptureTime:HH:mm:ss} ({_savedAutoTimeline.TotalDuration.TotalSeconds:F1}s, {_savedAutoTimeline.Events.Length} events)");
        sb.AppendLine();

        // Timeline comparison
        sb.AppendLine("┌─────────────────────────────────────────────────────────────────────────────────────┐");
        sb.AppendLine("│                              EVENT TIMELINE COMPARISON                              │");
        sb.AppendLine("├────────────────────────────────────┬────────────────────────────────────────────────┤");
        sb.AppendLine("│           MANUAL LOGIN             │               AUTO LOGIN                       │");
        sb.AppendLine("├────────────────────────────────────┼────────────────────────────────────────────────┤");

        // Interleave events by relative time
        var manualEvents = _savedManualTimeline.Events
            .Select(e => (Time: (e.Timestamp - _savedManualTimeline.CaptureTime + _savedManualTimeline.TotalDuration).TotalMilliseconds, Event: e, Source: "M"))
            .ToList();
        var autoEvents = _savedAutoTimeline.Events
            .Select(e => (Time: (e.Timestamp - _savedAutoTimeline.CaptureTime + _savedAutoTimeline.TotalDuration).TotalMilliseconds, Event: e, Source: "A"))
            .ToList();

        // Group events by category for comparison
        string[] categories = ["WorldInformation", "CheckUserLimitResult", "SelectWorldResult", "CUIWorldSelect", "CUIChannelSelect", "WorldId", "ChannelId", "RequestSent"];

        foreach (var category in categories)
        {
            var manualEvent = FindEventByCategory([.. manualEvents.Select(x => x.Event)], category, _savedManualTimeline.CaptureTime);
            var autoEvent = FindEventByCategory([.. autoEvents.Select(x => x.Event)], category, _savedAutoTimeline.CaptureTime);

            var manualStr = manualEvent != null ? $"+{manualEvent.Value.relMs,6:F0}ms {manualEvent.Value.desc}" : "---";
            var autoStr = autoEvent != null ? $"+{autoEvent.Value.relMs,6:F0}ms {autoEvent.Value.desc}" : "---";

            // Truncate to fit columns
            if (manualStr.Length > 34) manualStr = manualStr[..31] + "...";
            if (autoStr.Length > 44) autoStr = autoStr[..41] + "...";

            sb.AppendLine($"│ {manualStr,-34} │ {autoStr,-46} │");
        }

        sb.AppendLine("└────────────────────────────────────┴────────────────────────────────────────────────┘");
        sb.AppendLine();

        // Final state comparison
        sb.AppendLine("┌─────────────────────────────────────────────────────────────────────────────────────┐");
        sb.AppendLine("│                             FINAL STATE COMPARISON                                  │");
        sb.AppendLine("└─────────────────────────────────────────────────────────────────────────────────────┘");

        var ms = _savedManualTimeline.FinalState;
        var auto = _savedAutoTimeline.FinalState;

        if (ms != null && auto != null)
        {
            sb.AppendLine($"{"Field",-30} {"Manual",-15} {"Auto",-15} {"Match",-8}");
            sb.AppendLine(new string('-', 70));
            CompareField(sb, "CWvsContext.WorldId", ms.WorldId, auto.WorldId);
            CompareField(sb, "CWvsContext.ChannelId", ms.ChannelId, auto.ChannelId);
            CompareField(sb, "CWvsContext.CharacterCount", ms.CharacterCount, auto.CharacterCount);
            CompareField(sb, "CLogin.RequestSent", ms.RequestSent, auto.RequestSent);
            CompareField(sb, "CLogin.LoginStep", ms.LoginStep, auto.LoginStep);
            CompareField(sb, "CLogin.StepChanging", ms.StepChanging, auto.StepChanging);
            CompareField(sb, "CUIWorldSelect.Exists", ms.WorldSelectExists, auto.WorldSelectExists);
            CompareField(sb, "CUIWorldSelect.WorldIdx", ms.WorldIdx, auto.WorldIdx);
            CompareField(sb, "CUIChannelSelect.Exists", ms.ChannelSelectExists, auto.ChannelSelectExists);
            CompareField(sb, "CUIChannelSelect.Selected", ms.SelectedChannel, auto.SelectedChannel);
            CompareField(sb, "ConnectionDlg.Exists", ms.ConnectionDlgExists, auto.ConnectionDlgExists);
        }

        sb.AppendLine();

        // Analysis
        sb.AppendLine("┌─────────────────────────────────────────────────────────────────────────────────────┐");
        sb.AppendLine("│                               DIFFERENCE ANALYSIS                                   │");
        sb.AppendLine("└─────────────────────────────────────────────────────────────────────────────────────┘");

        var diffs = FindDifferences(ms, auto);
        if (diffs.Count == 0)
        {
            sb.AppendLine("  ✓ No significant differences detected between manual and auto login final states.");
        }
        else
        {
            sb.AppendLine("  Key differences that may cause the crash:");
            foreach (var diff in diffs)
            {
                sb.AppendLine($"  ✗ {diff}");
            }
        }

        return sb.ToString();
    }

    private static (double relMs, string desc)? FindEventByCategory(DiagnosticEvent[] events, string category, DateTime baseTime)
    {
        DiagnosticEvent? found = null;

        foreach (var evt in events)
        {
            if (category == "WorldInformation" && evt.Description.Contains("WorldInformation"))
            { found = evt; break; }
            if (category == "CheckUserLimitResult" && evt.Description.Contains("CheckUserLimitResult"))
            { found = evt; break; }
            if (category == "SelectWorldResult" && evt.Description.Contains("SelectWorldResult"))
            { found = evt; break; }
            if (category == "CUIWorldSelect" && evt.Details?.TryGetValue("changes", out var c1) == true &&
                c1 is List<string> l1 && l1.Any(s => s.Contains("CUIWorldSelect")))
            { found = evt; break; }
            if (category == "CUIChannelSelect" && evt.Details?.TryGetValue("changes", out var c2) == true &&
                c2 is List<string> l2 && l2.Any(s => s.Contains("CUIChannelSelect")))
            { found = evt; break; }
            if (category == "WorldId" && evt.Details?.TryGetValue("changes", out var c3) == true &&
                c3 is List<string> l3 && l3.Any(s => s.Contains("WorldId:")))
            { found = evt; break; }
            if (category == "ChannelId" && evt.Details?.TryGetValue("changes", out var c4) == true &&
                c4 is List<string> l4 && l4.Any(s => s.Contains("ChannelId:")))
            { found = evt; break; }
            if (category == "RequestSent" && evt.Details?.TryGetValue("changes", out var c5) == true &&
                c5 is List<string> l5 && l5.Any(s => s.Contains("RequestSent:")))
            { found = evt; break; }
        }

        if (found == null) return null;
        return ((found.Timestamp - baseTime).TotalMilliseconds, found.Description.Length > 30 ? found.Description[..30] : found.Description);
    }

    private static void CompareField<T>(StringBuilder sb, string name, T? manual, T? auto)
    {
        var manualStr = manual?.ToString() ?? "null";
        var autoStr = auto?.ToString() ?? "null";
        var match = Equals(manual, auto) ? "✓" : "✗ DIFF";
        sb.AppendLine($"{name,-30} {manualStr,-15} {autoStr,-15} {match,-8}");
    }

    private static List<string> FindDifferences(MemorySnapshot? manual, MemorySnapshot? auto)
    {
        var diffs = new List<string>();
        if (manual == null || auto == null) return diffs;

        // Critical fields
        if (manual.WorldId != auto.WorldId)
            diffs.Add($"WorldId: manual={manual.WorldId}, auto={auto.WorldId}");
        if (manual.ChannelId != auto.ChannelId)
            diffs.Add($"ChannelId: manual={manual.ChannelId}, auto={auto.ChannelId}");
        if (manual.RequestSent != auto.RequestSent)
            diffs.Add($"RequestSent: manual={manual.RequestSent}, auto={auto.RequestSent} - CRITICAL: must be true!");
        if (manual.WorldIdx != auto.WorldIdx)
            diffs.Add($"CUIWorldSelect.WorldIdx: manual={manual.WorldIdx}, auto={auto.WorldIdx} - affects CUIChannelSelect creation");
        if (manual.ChannelSelectExists != auto.ChannelSelectExists)
            diffs.Add($"CUIChannelSelect exists: manual={manual.ChannelSelectExists}, auto={auto.ChannelSelectExists}");
        if (manual.ConnectionDlgExists != auto.ConnectionDlgExists)
            diffs.Add($"ConnectionDlg exists: manual={manual.ConnectionDlgExists}, auto={auto.ConnectionDlgExists} - created by SendLoginPacket");
        if (manual.LoginStep != auto.LoginStep)
            diffs.Add($"LoginStep: manual={manual.LoginStep}, auto={auto.LoginStep}");

        return diffs;
    }

    public void LogPacketSent(string packetName, int opcode, Dictionary<string, object?>? details = null)
    {
        var evt = new DiagnosticEvent(
            DateTime.UtcNow,
            "Server",
            "PacketSent",
            $"[S->C] {packetName} (0x{opcode:X2})",
            details
        );
        _events.Enqueue(evt);
    }

    public void LogPacketReceived(string packetName, int opcode, Dictionary<string, object?>? details = null)
    {
        var evt = new DiagnosticEvent(
            DateTime.UtcNow,
            "Server",
            "PacketReceived",
            $"[C->S] {packetName} (0x{opcode:X2})",
            details
        );
        _events.Enqueue(evt);
    }

    public void LogAutoLoginStep(int step, string description, Dictionary<string, object?>? details = null)
    {
        var evt = new DiagnosticEvent(
            DateTime.UtcNow,
            "AutoLogin",
            "Step",
            $"[AutoLogin:{step}] {description}",
            details
        );
        _events.Enqueue(evt);
    }

    public void LogMemoryWrite(string field, object? oldValue, object? newValue)
    {
        var evt = new DiagnosticEvent(
            DateTime.UtcNow,
            "ClientMemory",
            "Write",
            $"[Memory] {field}: {oldValue} -> {newValue}",
            new Dictionary<string, object?> { ["field"] = field, ["old"] = oldValue, ["new"] = newValue }
        );
        _events.Enqueue(evt);
    }

    public void LogMemoryChange(MemorySnapshot? oldSnapshot, MemorySnapshot newSnapshot)
    {
        var changes = new List<string>();

        if (oldSnapshot == null)
        {
            changes.Add("Initial snapshot");
        }
        else
        {
            // CWvsContext changes
            if (oldSnapshot.AccountId != newSnapshot.AccountId)
                changes.Add($"AccountId: {oldSnapshot.AccountId} -> {newSnapshot.AccountId}");
            if (oldSnapshot.WorldId != newSnapshot.WorldId)
                changes.Add($"WorldId: {oldSnapshot.WorldId} -> {newSnapshot.WorldId}");
            if (oldSnapshot.ChannelId != newSnapshot.ChannelId)
                changes.Add($"ChannelId: {oldSnapshot.ChannelId} -> {newSnapshot.ChannelId}");
            if (oldSnapshot.CharacterCount != newSnapshot.CharacterCount)
                changes.Add($"CharacterCount: {oldSnapshot.CharacterCount} -> {newSnapshot.CharacterCount}");
            if (oldSnapshot.SlotCount != newSnapshot.SlotCount)
                changes.Add($"SlotCount: {oldSnapshot.SlotCount} -> {newSnapshot.SlotCount}");
            if (oldSnapshot.ChannelNameArrayPtr != newSnapshot.ChannelNameArrayPtr)
                changes.Add($"ChannelNamePtr: {FormatPtr(oldSnapshot.ChannelNameArrayPtr)} -> {FormatPtr(newSnapshot.ChannelNameArrayPtr)}");
            if (oldSnapshot.AdultChannelArrayPtr != newSnapshot.AdultChannelArrayPtr)
                changes.Add($"AdultChannelPtr: {FormatPtr(oldSnapshot.AdultChannelArrayPtr)} -> {FormatPtr(newSnapshot.AdultChannelArrayPtr)}");

            // CLogin changes
            if (oldSnapshot.RequestSent != newSnapshot.RequestSent)
                changes.Add($"RequestSent: {oldSnapshot.RequestSent} -> {newSnapshot.RequestSent}");
            if (oldSnapshot.LoginStep != newSnapshot.LoginStep)
                changes.Add($"LoginStep: {oldSnapshot.LoginStep} -> {newSnapshot.LoginStep}");
            if (oldSnapshot.StepChanging != newSnapshot.StepChanging)
                changes.Add($"StepChanging: {oldSnapshot.StepChanging} -> {newSnapshot.StepChanging}");
            if (oldSnapshot.CharSelected != newSnapshot.CharSelected)
                changes.Add($"CharSelected: {oldSnapshot.CharSelected} -> {newSnapshot.CharSelected}");

            // CUIChannelSelect changes
            if (oldSnapshot.ChannelSelectExists != newSnapshot.ChannelSelectExists)
                changes.Add($"CUIChannelSelect: {(oldSnapshot.ChannelSelectExists ? "exists" : "null")} -> {(newSnapshot.ChannelSelectExists ? "exists" : "null")}");
            if (oldSnapshot.SelectedChannel != newSnapshot.SelectedChannel)
                changes.Add($"SelectedChannel: {oldSnapshot.SelectedChannel} -> {newSnapshot.SelectedChannel}");
            if (oldSnapshot.WorldItemPtr != newSnapshot.WorldItemPtr)
                changes.Add($"WorldItemPtr: {FormatPtr(oldSnapshot.WorldItemPtr)} -> {FormatPtr(newSnapshot.WorldItemPtr)}");
            if (oldSnapshot.ConnectionDlgExists != newSnapshot.ConnectionDlgExists)
                changes.Add($"ConnectionDlg: {(oldSnapshot.ConnectionDlgExists ? "exists" : "null")} -> {(newSnapshot.ConnectionDlgExists ? "exists" : "null")}");

            // CUIWorldSelect changes
            if (oldSnapshot.WorldSelectExists != newSnapshot.WorldSelectExists)
                changes.Add($"CUIWorldSelect: {(oldSnapshot.WorldSelectExists ? "exists" : "null")} -> {(newSnapshot.WorldSelectExists ? "exists" : "null")}");
            if (oldSnapshot.WorldIdx != newSnapshot.WorldIdx)
                changes.Add($"WorldIdx: {oldSnapshot.WorldIdx} -> {newSnapshot.WorldIdx}");
        }

        if (changes.Count > 0)
        {
            var evt = new DiagnosticEvent(
                newSnapshot.Timestamp,
                "ClientMemory",
                "StateChange",
                $"[Memory] {changes.Count} field(s) changed",
                new Dictionary<string, object?> { ["changes"] = changes }
            );
            _events.Enqueue(evt);
        }

        lock (_snapshotLock)
        {
            _lastSnapshot = newSnapshot;
        }
    }

    private static string FormatPtr(int? ptr)
        => ptr.HasValue ? $"0x{ptr.Value:X8}" : "null";

    public void LogError(string source, string message, Exception? ex = null)
    {
        var evt = new DiagnosticEvent(
            DateTime.UtcNow,
            source,
            "Error",
            $"[ERROR] {message}",
            ex != null ? new Dictionary<string, object?> { ["exception"] = ex.ToString() } : null
        );
        _events.Enqueue(evt);
    }

    public void LogInfo(string source, string message)
    {
        var evt = new DiagnosticEvent(
            DateTime.UtcNow,
            source,
            "Info",
            message,
            null
        );
        _events.Enqueue(evt);
    }

    public MemorySnapshot? GetLastSnapshot()
    {
        lock (_snapshotLock)
        {
            return _lastSnapshot;
        }
    }

    public IReadOnlyList<DiagnosticEvent> GetEvents() => [.. _events];

    public string GenerateReport()
    {
        var sb = new StringBuilder();
        var events = _events.ToArray();

        sb.AppendLine("╔════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║           v95 CLIENT LOGIN FLOW DIAGNOSTIC REPORT                  ║");
        sb.AppendLine("╠════════════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  Start Time: {StartTime:yyyy-MM-dd HH:mm:ss.fff}                         ║");
        sb.AppendLine($"║  Events:     {events.Length}                                                    ║");
        sb.AppendLine("╚════════════════════════════════════════════════════════════════════╝");
        sb.AppendLine();

        sb.AppendLine("┌────────────────────────────────────────────────────────────────────┐");
        sb.AppendLine("│                         EVENT TIMELINE                             │");
        sb.AppendLine("└────────────────────────────────────────────────────────────────────┘");

        foreach (var evt in events)
        {
            var relativeMs = (evt.Timestamp - StartTime).TotalMilliseconds;
            var sourceTag = evt.Source switch
            {
                "Server" => "[SVR]",
                "ClientMemory" => "[MEM]",
                "AutoLogin" => "[AUTO]",
                _ => "[???]"
            };

            sb.AppendLine($"[+{relativeMs,8:F1}ms] {sourceTag,-6} {evt.Description}");

            if (evt.Details != null && evt.Details.Count > 0)
            {
                foreach (var (key, value) in evt.Details)
                {
                    if (key == "changes" && value is List<string> changes)
                    {
                        foreach (var change in changes)
                        {
                            sb.AppendLine($"               └─ {change}");
                        }
                    }
                    else if (key != "exception")
                    {
                        sb.AppendLine($"               └─ {key}: {value}");
                    }
                }
            }
        }

        // Show crash-time state if available (captured before client cleanup)
        var crashSnapshot = GetCrashSnapshot();
        if (crashSnapshot != null)
        {
            sb.AppendLine();
            sb.AppendLine("┌────────────────────────────────────────────────────────────────────┐");
            sb.AppendLine("│                  STATE AT CRASH TIME                               │");
            sb.AppendLine("│  (Captured before client cleanup - this is the important state)    │");
            sb.AppendLine("└────────────────────────────────────────────────────────────────────┘");
            sb.Append(crashSnapshot.Describe());
        }

        sb.AppendLine();
        sb.AppendLine("┌────────────────────────────────────────────────────────────────────┐");
        sb.AppendLine("│                    FINAL CLIENT STATE                              │");
        sb.AppendLine("└────────────────────────────────────────────────────────────────────┘");

        var lastSnapshot = GetLastSnapshot();
        if (lastSnapshot != null)
        {
            if (crashSnapshot != null)
            {
                sb.AppendLine("(Note: This is the state AFTER crash cleanup - may show null values");
                sb.AppendLine(" as the client destroys UI singletons when returning to title screen)");
                sb.AppendLine();
            }
            sb.Append(lastSnapshot.Describe());
        }
        else
        {
            sb.AppendLine("No client memory snapshot available.");
        }

        sb.AppendLine();
        sb.AppendLine("┌────────────────────────────────────────────────────────────────────┐");
        sb.AppendLine("│                    EXPECTED vs ACTUAL FLOW                         │");
        sb.AppendLine("└────────────────────────────────────────────────────────────────────┘");
        sb.AppendLine(GenerateFlowComparison(events));

        sb.AppendLine();
        sb.AppendLine("┌────────────────────────────────────────────────────────────────────┐");
        sb.AppendLine("│                         ANALYSIS                                   │");
        sb.AppendLine("└────────────────────────────────────────────────────────────────────┘");
        // Use crash snapshot for analysis if available (more accurate than post-cleanup state)
        var analysisSnapshot = crashSnapshot ?? lastSnapshot;
        sb.AppendLine(AnalyzeFlow(events, analysisSnapshot));

        return sb.ToString();
    }

    private string GenerateFlowComparison(DiagnosticEvent[] events)
    {
        var sb = new StringBuilder();

        // Define expected flow steps (covers both manual and auto login)
        // Steps marked with (manual) only occur during manual login
        // Steps marked with (auto) only occur during auto login
        var expectedSteps = new[]
        {
            ("CheckPasswordResult", "[S→C] CheckPasswordResult sent"),
            ("WorldRequest", "[C→S] WorldRequest received"),
            ("WorldInformation", "[S→C] WorldInformation sent"),
            ("CUIWorldSelectCreated", "[MEM] CUIWorldSelect created"),
            ("CheckUserLimit", "[C→S] CheckUserLimit (user clicked world)"),
            ("CheckUserLimitResult", "[S→C] CheckUserLimitResult sent"),
            ("CUIChannelSelectCreated", "[MEM] CUIChannelSelect created"),
            ("SelectWorld", "[C→S] SelectWorld (user clicked channel)"),
            ("MemoryWriteWorldChannel", "[MEM] CWvsContext World/Channel written (auto)"),
            ("MemoryWriteRequestSent", "[MEM] CLogin->m_bRequestSent written (auto)"),
            ("SelectWorldResult", "[S→C] SelectWorldResult sent"),
            ("CharCountSet", "[MEM] CharacterCount set by client"),
            ("SelectCharacterState", "Client in SelectCharacter state")
        };

        // Check which steps occurred
        foreach (var (stepId, description) in expectedSteps)
        {
            var occurred = false;
            DateTime? timestamp = null;

            // Check milestones first
            if (FlowMilestones.TryGetValue(stepId, out var milestoneTime))
            {
                occurred = true;
                timestamp = milestoneTime;
            }
            else
            {
                // Check events
                var matchingEvent = FindMatchingEvent(events, stepId);
                if (matchingEvent != null)
                {
                    occurred = true;
                    timestamp = matchingEvent.Timestamp;
                }
            }

            var status = occurred ? "✓" : "✗";
            var timeStr = timestamp.HasValue
                ? $"+{(timestamp.Value - StartTime).TotalMilliseconds,8:F0}ms"
                : "         -";

            sb.AppendLine($"  {status} {timeStr} │ {description}");
        }

        return sb.ToString();
    }

    private static DiagnosticEvent? FindMatchingEvent(DiagnosticEvent[] events, string stepId)
    {
        return stepId switch
        {
            "CheckPasswordResult" => events.FirstOrDefault(e =>
                e.Description.Contains("CheckPasswordResult") && e.Description.Contains("S->C")),
            "WorldRequest" => events.FirstOrDefault(e =>
                e.Description.Contains("WorldRequest") && e.Description.Contains("C->S")),
            "WorldInformation" => events.FirstOrDefault(e =>
                e.Description.Contains("WorldInformation") && e.Description.Contains("S->C")),
            "CUIWorldSelectCreated" => events.FirstOrDefault(e =>
                e.Source == "ClientMemory" &&
                e.Details?.TryGetValue("changes", out var c) == true &&
                c is List<string> list && list.Any(s => s.Contains("CUIWorldSelect") && s.Contains("exists"))),
            "CheckUserLimit" => events.FirstOrDefault(e =>
                e.Description.Contains("CheckUserLimit") && e.Description.Contains("C->S") &&
                !e.Description.Contains("Result")),
            "CheckUserLimitResult" => events.FirstOrDefault(e =>
                e.Description.Contains("CheckUserLimitResult") && e.Description.Contains("S->C")),
            "CUIChannelSelectCreated" => events.FirstOrDefault(e =>
                e.Source == "ClientMemory" &&
                e.Details?.TryGetValue("changes", out var c) == true &&
                c is List<string> list && list.Any(s => s.Contains("CUIChannelSelect") && s.Contains("exists"))),
            "SelectWorld" => events.FirstOrDefault(e =>
                e.Description.Contains("SelectWorld") && e.Description.Contains("C->S") &&
                !e.Description.Contains("Result")),
            "MemoryWriteWorldChannel" => events.FirstOrDefault(e =>
                e.Source == "ClientMemory" && e.EventType == "Write" &&
                e.Description.Contains("m_nWorldID")),
            "MemoryWriteRequestSent" => events.FirstOrDefault(e =>
                e.Source == "ClientMemory" && e.EventType == "Write" &&
                e.Description.Contains("m_bRequestSent")),
            "SelectWorldResult" => events.FirstOrDefault(e =>
                e.Description.Contains("SelectWorldResult") && e.Description.Contains("S->C")),
            "CharCountSet" => events.FirstOrDefault(e =>
                e.Source == "ClientMemory" &&
                e.Details?.TryGetValue("changes", out var c) == true &&
                c is List<string> list && list.Any(s => s.Contains("CharacterCount") && !s.Contains("-> 0") && !s.Contains("->0"))),
            _ => null
        };
    }

    private static string AnalyzeFlow(DiagnosticEvent[] events, MemorySnapshot? finalState)
    {
        var sb = new StringBuilder();
        var issues = new List<string>();
        var observations = new List<string>();

        // Check for critical state
        if (finalState != null)
        {
            // Note: WorldId=0 and ChannelId=0 are VALID (first world, first channel)
            // Only flag as issue if they are NULL (never set)
            if (finalState.WorldId == null)
                issues.Add("CRITICAL: CWvsContext->m_nWorldID is null - was never set");
            else
                observations.Add($"CWvsContext->m_nWorldID = {finalState.WorldId} (valid)");

            if (finalState.ChannelId == null)
                issues.Add("CRITICAL: CWvsContext->m_nChannelID is null - was never set");
            else
                observations.Add($"CWvsContext->m_nChannelID = {finalState.ChannelId} (valid)");

            // CLogin state analysis
            if (finalState.RequestSent == null)
                issues.Add("WARNING: CLogin->m_bRequestSent is null - CLogin may not be accessible");
            else if (finalState.RequestSent == false)
                issues.Add("WARNING: CLogin->m_bRequestSent is false - should be true before SelectWorldResult");
            else
                observations.Add("CLogin->m_bRequestSent = true (correct)");

            // Check for garbage CLogin values (indicates wrong pointer)
            if (finalState.LoginStep != null && (finalState.LoginStep < 0 || finalState.LoginStep > 10))
                issues.Add($"CRITICAL: CLogin->m_nLoginStep={finalState.LoginStep} looks like garbage - wrong CLogin address?");

            // m_tStepChanging is set to GetTickCount() when a step transition starts.
            // Values like 7530671 (~2 hours uptime) are normal, not garbage.

            // UI state analysis
            if (!finalState.ChannelSelectExists)
                issues.Add("WARNING: CUIChannelSelect does not exist - CheckUserLimitResult not processed?");
            else
                observations.Add("CUIChannelSelect exists (correct)");

            if (!finalState.WorldSelectExists)
                issues.Add("WARNING: CUIWorldSelect does not exist - WorldInformation not processed?");
            else
                observations.Add("CUIWorldSelect exists (correct)");

            if (finalState.StepChanging != null && finalState.StepChanging > 0)
                observations.Add($"m_tStepChanging={finalState.StepChanging} (GetTickCount - client transitioning between steps)");

            // ConnectionDlg state - normally created by SendLoginPacket's DrawNoticeConnecting
            if (finalState.ConnectionDlgExists)
                observations.Add("ConnectionDlg exists (\"Connecting...\" dialog visible)");
        }

        // Analyze event sequence
        var memoryEvents = events.Where(e => e.Source == "ClientMemory").ToArray();
        var packetEvents = events.Where(e => e.Source == "Server").ToArray();

        // Find key events by checking Details
        var worldSelectCreated = memoryEvents.FirstOrDefault(e =>
            e.Details?.TryGetValue("changes", out var changes) == true &&
            changes is List<string> list && list.Any(c => c.Contains("CUIWorldSelect") && c.Contains("exists")));

        var channelSelectCreated = memoryEvents.FirstOrDefault(e =>
            e.Details?.TryGetValue("changes", out var changes) == true &&
            changes is List<string> list && list.Any(c => c.Contains("CUIChannelSelect") && c.Contains("exists")));

        var crashEvent = events.FirstOrDefault(e => e.Description.Contains("crash") || e.Description.Contains("ClientDumpLog"));

        // Timing analysis
        if (worldSelectCreated != null && channelSelectCreated != null)
        {
            var timeBetween = (channelSelectCreated.Timestamp - worldSelectCreated.Timestamp).TotalMilliseconds;
            observations.Add($"CUIChannelSelect created {timeBetween:F0}ms after CUIWorldSelect");
        }

        if (crashEvent != null)
        {
            // Find the crash packet type
            if (crashEvent.Details?.TryGetValue("type", out var crashType) == true)
            {
                var typeStr = crashType?.ToString() ?? "";
                if (typeStr.Contains("0xB") || typeStr.Contains("11"))
                    issues.Add("CRASH in SelectWorldResult (0x0B) handler - check OnSelectWorldResult flow");
                else if (typeStr.Contains("0x3") || typeStr.Contains('3'))
                    issues.Add("CRASH in CheckUserLimitResult (0x03) handler - CUIWorldSelect may not exist");
            }
        }

        // Build output
        if (observations.Count > 0)
        {
            sb.AppendLine("Observations:");
            foreach (var obs in observations)
            {
                sb.AppendLine($"  ✓ {obs}");
            }
            sb.AppendLine();
        }

        if (issues.Count == 0)
        {
            sb.AppendLine("No obvious issues detected in the login flow.");
        }
        else
        {
            sb.AppendLine("Issues detected:");
            foreach (var issue in issues)
            {
                sb.AppendLine($"  ✗ {issue}");
            }
        }

        return sb.ToString();
    }
}
