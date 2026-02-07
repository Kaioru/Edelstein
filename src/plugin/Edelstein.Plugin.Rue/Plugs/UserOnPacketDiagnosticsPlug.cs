using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Plugin.Rue.ClientAnalysis;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Diagnostics;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue.Plugs;

/// <summary>
/// Comprehensive diagnostics plug that monitors both client memory state and packet flow
/// to help diagnose auto-login issues in v95 clients.
///
/// ## Features:
/// - Tracks incoming packets (C->S) with timestamps
/// - Monitors client memory state changes at configurable intervals
/// - Correlates packet events with memory state transitions
/// - Generates diagnostic reports on ClientDumpLog (crash) packets
///
/// ## Key monitored structures:
/// - CWvsContext: m_nWorldID, m_nChannelID, m_dwAccountId, m_nCharacterCount
/// - CLogin: m_bRequestSent, m_nLoginStep, m_tStepChanging, m_nCharSelected
/// - CUIChannelSelect: existence, m_nSelect, m_pConnectionDlg
/// - CUIWorldSelect: existence, m_nWorldIdx
/// </summary>
public sealed class UserOnPacketDiagnosticsPlug(
    ILogger? logger,
    IOptions<RueConfigLogin> options,
    LoginDiagnostics? sharedDiagnostics = null,
    MemoryContext? memoryContext = null)
    : IPipelinePlug<UserOnPacket<ILoginStageUser>>
{
    private readonly ILogger? _logger = logger;
    private readonly RueConfigLogin _config = options.Value;
    private readonly LoginDiagnostics _diagnostics = sharedDiagnostics ?? new LoginDiagnostics();

    private readonly MemoryContext? _memoryContext = memoryContext;
    private Timer? _watchTimer;
    private bool _watchStarted;
    private DateTime _watchStartTime;
    private MemorySnapshot? _lastSnapshot;

    private const int MaxCrashBufferBytes = 512;
    private const int MinWatchIntervalMs = 10;
    private const int MinWatchDurationMs = 1000;
    private const string CrashReportPrefixManual = "crash_manual";
    private const string CrashReportPrefixAuto = "crash_auto";
    private const string CompleteReportPrefixManual = "complete_manual";
    private const string CompleteReportPrefixAuto = "complete_auto";

    /// <summary>
    /// Gets the diagnostics instance for external access (e.g., generating reports).
    /// </summary>
    public LoginDiagnostics Diagnostics => _diagnostics;

    public Task Handle(IPipelineContext ctx, UserOnPacket<ILoginStageUser> message)
    {
        if (!_config.DiagnosticsEnabled)
            return Task.CompletedTask;

        MemoryWriter? writer = null;
        if (_config.ClientMemory?.Enabled ?? false)
            writer = EnsureWriter();

        using var reader = new PacketReader(message.Packet.Buffer);
        var op = reader.ReadShort();

        var opName = Enum.GetName((PacketRecvOperations)op) ?? "UNKNOWN";

        if (_config.ClientMemory?.WatchEnabled ?? false)
            EnsureWatchStarted();

        // Special handling for crash dump packets (before generic logging to avoid duplication)
        if (op == (short)PacketRecvOperations.ClientDumpLog)
        {
            HandleClientDumpLog(reader);
            ctx.Cancel();
            return Task.CompletedTask;
        }

        // Special handling for exception log packets (before generic logging to avoid duplication)
        if (op == (short)PacketRecvOperations.ExceptionLog)
        {
            HandleExceptionLog(reader, message);
            ctx.Cancel();
            return Task.CompletedTask;
        }

        // Log relevant login packets (excludes ClientDumpLog and ExceptionLog which have dedicated handlers)
        if (IsLoginRelatedPacket(op))
        {
            RecordPacketMilestone(op);

            var snapshot = writer != null ? CreateSnapshot(writer) : null;
            var clientStep = snapshot?.LoginStep;
            var clientStepName = clientStep.HasValue
                ? Enum.GetName(typeof(LoginStep), clientStep.Value) ?? clientStep.Value.ToString()
                : null;

            var clientStepLabel = clientStep.HasValue
                ? (clientStepName != null ? $"{clientStep}({clientStepName})" : clientStep.Value.ToString())
                : "unavailable";

            var details = new Dictionary<string, object?>
            {
                ["srvState"] = message.User.State.ToString(),
                ["selectedWorld"] = message.User.SelectedWorldID,
                ["selectedChannel"] = message.User.SelectedChannelID,
                ["accountId"] = message.User.Account?.ID,
                ["cliStep"] = clientStep,
                ["cliStepName"] = clientStepName,
                ["cliStepChanging"] = snapshot?.StepChanging,
                ["cliWorldId"] = snapshot?.WorldId,
                ["cliChannelId"] = snapshot?.ChannelId
            };

            _diagnostics.LogPacketReceived(opName, op, details);

            _logger?.LogInformation(
                "[Diag] C->S 0x{Op:X2}({OpName}) srv={State} accId={AccountId} sel=({World},{Ch}) cli={Cli} stepChg={StepChg}",
                op,
                opName,
                message.User.State,
                message.User.Account?.ID,
                message.User.SelectedWorldID,
                message.User.SelectedChannelID,
                clientStepLabel,
                snapshot?.StepChanging
            );
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Logs an outgoing packet (S->C) for diagnostics.
    /// Call this from other plugs when sending important packets.
    /// </summary>
    public void LogPacketSent(string packetName, int opcode, Dictionary<string, object?>? details = null)
    {
        var enabled = _config.DiagnosticsEnabled;
        if (!enabled)
            return;

        _diagnostics.LogPacketSent(packetName, opcode, details);

        _logger?.LogInformation(
            "[Diag] [S->C] 0x{Op:X2} {PacketName}",
            opcode,
            packetName
        );
    }

    /// <summary>
    /// Logs an auto-login step for diagnostics.
    /// </summary>
    public void LogAutoLoginStep(int step, string description, Dictionary<string, object?>? details = null)
    {
        var enabled = _config.DiagnosticsEnabled;
        if (!enabled)
            return;

        _diagnostics.LogAutoLoginStep(step, description, details);
    }

    /// <summary>
    /// Generates a diagnostic report and logs it.
    /// </summary>
    public string GenerateReport()
    {
        // Take a final snapshot
        var writer = EnsureWriter();
        if (writer != null)
        {
            try
            {
                var snapshot = CreateSnapshot(writer);
                _diagnostics.LogMemoryChange(_diagnostics.GetLastSnapshot(), snapshot);
            }
            catch (Exception ex)
            {
                _diagnostics.LogError("Diagnostics", $"Failed to read final snapshot: {ex.Message}");
            }
        }

        var report = _diagnostics.GenerateReport();
        _logger?.LogInformation("{Report}", report);
        return report;
    }

    private MemoryWriter? EnsureWriter()
    {
        var memConfig = _config.ClientMemory;
        if (memConfig == null)
            return null;

        if (_memoryContext == null)
            return null;

        if (!_memoryContext.TryInitialize(memConfig, _logger, _diagnostics, _config.DiagnosticsEnabled))
            return null;

        return _memoryContext.Writer;
    }

    private static MemorySnapshot CreateSnapshot(MemoryWriter writer)
    {
        return new MemorySnapshot(
            DateTime.UtcNow,
            writer.ReadAccountId(),
            writer.ReadWorldId(),
            writer.ReadChannelId(),
            writer.ReadCharacterCount(),
            writer.ReadSlotCount(),
            writer.ReadChannelNameArrayPtr(),
            writer.ReadAdultChannelArrayPtr(),
            writer.ReadRequestSent(),
            writer.ReadLoginStep(),
            writer.ReadStepChanging(),
            writer.ReadCharSelected(),
            writer.ReadLoginOpt(),
            writer.IsCUIChannelSelectValid() ?? false,
            writer.ReadChannelSelectSelected(),
            writer.ReadChannelSelectWorldItemPtr(),
            writer.IsConnectionDlgValid() ?? false,
            writer.IsCUIWorldSelectValid() ?? false,
            writer.ReadWorldSelectWorldIdx()
        );
    }

    private void RecordPacketMilestone(short op)
    {
        if (!_config.DiagnosticsEnabled)
            return;

        if (op == (short)PacketRecvOperations.WorldRequest)
            _diagnostics.RecordMilestone("WorldRequest");
        else if (op == (short)PacketRecvOperations.CheckUserLimit)
            _diagnostics.RecordMilestone("CheckUserLimit");
        else if (op == (short)PacketRecvOperations.SelectWorld)
            _diagnostics.RecordMilestone("SelectWorld");
    }

    private void RecordSnapshotMilestones(MemorySnapshot? prev, MemorySnapshot current)
    {
        if (!_config.DiagnosticsEnabled)
            return;

        if (prev == null)
            return;

        var p = prev;

        if (!p.WorldSelectExists && current.WorldSelectExists)
            _diagnostics.RecordMilestone("CUIWorldSelectCreated");

        if (!p.ChannelSelectExists && current.ChannelSelectExists)
            _diagnostics.RecordMilestone("CUIChannelSelectCreated");

        if ((p.CharacterCount ?? 0) == 0 && (current.CharacterCount ?? 0) > 0)
            _diagnostics.RecordMilestone("CharCountSet");

        if (current.LoginStep == (int)LoginStep.SelectCharacter && (current.StepChanging ?? 0) == 0)
            _diagnostics.RecordMilestone("SelectCharacterState");
    }

    private static bool IsLoginRelatedPacket(short op)
    {
        // Note: ClientDumpLog and ExceptionLog are excluded here because they have
        // dedicated handlers (HandleClientDumpLog/HandleExceptionLog) and are handled
        // before this check to avoid duplicate logging.
        return op is (short)PacketRecvOperations.CheckPassword
            or (short)PacketRecvOperations.WorldInfoRequest
            or (short)PacketRecvOperations.WorldRequest
            or (short)PacketRecvOperations.CheckUserLimit
            or (short)PacketRecvOperations.SelectWorld
            or (short)PacketRecvOperations.LogoutWorld
            or (short)PacketRecvOperations.CheckSPWRequest
            or (short)PacketRecvOperations.EnableSPWRequest
            or (short)PacketRecvOperations.CreateNewCharacter
            or (short)PacketRecvOperations.DeleteCharacter;
    }

    private void HandleClientDumpLog(PacketReader reader)
    {
        if (reader.Available < 2 + 4 + 2 + 4 + 2)
        {
            _diagnostics.LogError("ClientDumpLog", "Packet too short to parse");
            return;
        }

        var callType = reader.ReadShort();
        var errorCode = reader.ReadInt();
        var backupBufferSize = reader.ReadShort();
        var rawSeq = reader.ReadInt();
        var type = reader.ReadShort();
        var remaining = Math.Max(0, backupBufferSize - 6);
        var buffer = remaining <= reader.Available
            ? reader.ReadBytes((short)remaining)
            : reader.ReadBytes((short)Math.Min(reader.Available, MaxCrashBufferBytes));

        var crashCallType = (CrashCallType)callType;
        var callTypeName = Enum.IsDefined(crashCallType)
            ? $"CRASH_{crashCallType.ToString().ToUpperInvariant()}"
            : $"CRASH_TYPE_{callType}";

        var packetOpName = Enum.GetName((PacketSendOperations)type) ?? $"Unknown(0x{type:X2})";

        var details = new Dictionary<string, object?>
        {
            ["callType"] = $"{callType} ({callTypeName})",
            ["errorCode"] = $"0x{errorCode:X8} ({CrashAnalyzer.GetNTStatusName(errorCode)})",
            ["rawSeq"] = $"0x{rawSeq:X8}",
            ["type"] = $"0x{type:X2}",
            ["packetName"] = packetOpName,
            ["bufferSize"] = buffer.Length,
            ["bufferHex"] = Convert.ToHexString(buffer),
            ["bufferAscii"] = HexFormatter.FormatAsciiDump(buffer)
        };

        // Try to parse additional info from the buffer based on packet type
        CrashAnalyzer.ParseCrashBuffer(type, buffer.AsSpan(), details);

        _diagnostics.LogPacketReceived("ClientDumpLog", (int)PacketRecvOperations.ClientDumpLog, details);

        _logger?.LogWarning(
            "[Diag] ========== CLIENT CRASH DETECTED ==========\n" +
            "  CallType: {CallType} ({CallTypeName})\n" +
            "  ErrorCode: 0x{Error:X8} ({ErrorName})\n" +
            "  RawSeq: 0x{Seq:X8}\n" +
            "  PacketType: 0x{Type:X2} ({PacketName})\n" +
            "  BufferSize: {BufferSize} bytes",
            callType, callTypeName,
            errorCode, CrashAnalyzer.GetNTStatusName(errorCode),
            rawSeq,
            type, packetOpName,
            buffer.Length
        );

        // Log buffer hex dump for debugging
        if (buffer.Length > 0)
        {
            _logger?.LogDebug("[Diag] Crash buffer hex dump:\n{HexDump}", HexFormatter.FormatHexDump(buffer));
        }

        // Auto-generate report on crash
        if (_config.DiagnosticsEnabled)
        {
            // Capture the crash snapshot BEFORE generating report - this preserves
            // the state at crash time before the client cleans up and nulls everything
            _diagnostics.SaveCrashSnapshot();

            _diagnostics.LogError("Client", $"Client crash: {callTypeName} while processing {packetOpName}");
            var report = GenerateReport();

            // Log crash analysis based on packet type
            var crashAnalysis = CrashAnalyzer.AnalyzeCrash(type, errorCode);
            _logger?.LogWarning("[Diag] CRASH ANALYSIS:\n{Analysis}", crashAnalysis);

            // Auto-save timeline based on mode
            SaveTimelineForMode();

            // Save the crash report to file
            var watchMode = _config.ClientMemory?.WatchMode ?? RueConfigClientMemory.WatchModeActive;
            var reportPrefix = watchMode.Equals(RueConfigClientMemory.WatchModePassive, StringComparison.OrdinalIgnoreCase)
                ? CrashReportPrefixManual
                : CrashReportPrefixAuto;
            var reportPath = LoginDiagnostics.SaveReportToFile(report + "\n\n" + crashAnalysis, reportPrefix);
            _logger?.LogInformation("[Diag] Crash report saved to: {Path}", reportPath);

            // If both timelines exist, generate and save comparison
            if (LoginDiagnostics.HasBothTimelines)
            {
                var comparison = LoginDiagnostics.GenerateComparison();
                _logger?.LogInformation("[Diag] MANUAL vs AUTO COMPARISON:\n{Comparison}", comparison);

                var comparisonPath = LoginDiagnostics.SaveComparisonReport();
                _logger?.LogInformation("[Diag] Comparison report saved to: {Path}", comparisonPath);
            }
        }
    }

    /// <summary>
    /// Saves the current timeline based on watch mode (passive=manual, active=auto).
    /// </summary>
    private void SaveTimelineForMode()
    {
        var watchMode = _config.ClientMemory?.WatchMode ?? RueConfigClientMemory.WatchModeActive;
        if (watchMode.Equals(RueConfigClientMemory.WatchModePassive, StringComparison.OrdinalIgnoreCase))
        {
            _diagnostics.SaveAsManualBaseline();
            _logger?.LogInformation("[Diag] Saved timeline as MANUAL baseline for comparison");
        }
        else
        {
            _diagnostics.SaveAsAutoAttempt();
            _logger?.LogInformation("[Diag] Saved timeline as AUTO attempt for comparison");
        }
    }

    private void HandleExceptionLog(PacketReader reader, UserOnPacket<ILoginStageUser> message)
    {
        // ExceptionLog packet structure:
        // string - exception message/call stack
        // The client sends this when it catches a C++ exception
        try
        {
            var exceptionText = reader.ReadString();

            var isAccessViolation = exceptionText.Contains("ACCESS_VIOLATION", StringComparison.OrdinalIgnoreCase)
                || exceptionText.Contains("0xC0000005", StringComparison.OrdinalIgnoreCase);

            var writer = EnsureWriter();
            var details = new Dictionary<string, object?>
            {
                ["exceptionText"] = exceptionText,
                ["srvState"] = message.User.State.ToString(),
                ["accountId"] = message.User.Account?.ID,
                ["accessViolation"] = isAccessViolation,
                ["cliStep"] = writer?.ReadLoginStep(),
                ["cliStepChanging"] = writer?.ReadStepChanging()
            };

            // Log as a single event (no separate LogError to avoid duplication)
            _diagnostics.LogPacketReceived("ExceptionLog", (int)PacketRecvOperations.ExceptionLog, details);

            var accountId = message.User.Account?.ID;
            if (accountId.HasValue)
            {
                _logger?.LogWarning(
                    "[Diag] ExceptionLog (srv={State}, accId={AccountId}, accessViolation={IsAccessViolation})",
                    message.User.State,
                    accountId,
                    isAccessViolation);
            }
            else
            {
                _logger?.LogDebug(
                    "[Diag] ExceptionLog pre-login (srv={State}, accessViolation={IsAccessViolation})",
                    message.User.State,
                    isAccessViolation);
            }

            _logger?.LogDebug("[Diag] ExceptionLog text:\n{ExceptionText}", exceptionText);
        }
        catch (Exception ex)
        {
            _diagnostics.LogError("ExceptionLog", $"Failed to parse ExceptionLog packet: {ex.Message}");
        }
    }

    private void EnsureWatchStarted()
    {
        if (_watchStarted)
            return;

        if (_config.ClientMemory == null)
        {
            _diagnostics.LogError("ClientMemory", "Watch enabled but ClientMemory config missing");
            return;
        }

        var writer = EnsureWriter();
        if (writer == null)
        {
            _diagnostics.LogError("ClientMemory", "Failed to initialize memory writer");
            return;
        }

        _watchStarted = true;
        _watchStartTime = DateTime.UtcNow;

        var intervalMs = Math.Max(MinWatchIntervalMs, _config.ClientMemory.WatchIntervalMs);
        var durationMs = Math.Max(MinWatchDurationMs, _config.ClientMemory.WatchDurationMs);

        // Take initial snapshot
        try
        {
            var initialSnapshot = CreateSnapshot(writer);
            _lastSnapshot = initialSnapshot;
            _diagnostics.LogMemoryChange(null, initialSnapshot);

            _logger?.LogInformation(
                "[Diag] Memory watch started - interval={Interval}ms, duration={Duration}ms",
                intervalMs,
                durationMs
            );

            LogSnapshotToLogger("Initial", initialSnapshot);
        }
        catch (Exception ex)
        {
            _diagnostics.LogError("ClientMemory", $"Failed to read initial snapshot: {ex.Message}");
        }

        // Start polling timer
        _watchTimer = new Timer(
            OnWatchTick,
            null,
            TimeSpan.FromMilliseconds(intervalMs),
            TimeSpan.FromMilliseconds(intervalMs)
        );

        // Stop after duration
        _ = Task.Run(async () =>
        {
            await Task.Delay(durationMs);
            StopWatch();
        });
    }

    private void OnWatchTick(object? state)
    {
        var writer = EnsureWriter();
        if (writer == null)
            return;

        // Stop polling if the client process has exited
        if (writer.HasProcessExited)
        {
            _diagnostics.LogInfo("ClientMemory", "[Memory] Client process exited - stopping watch");
            StopWatch();
            return;
        }

        try
        {
            // Re-resolve pointers each tick - CLogin only becomes available after
            // CUIChannelSelect is created (which happens after CheckUserLimitResult).
            // CLogin fields will be null until then.
            writer.RefreshPointers();

            var current = CreateSnapshot(writer);

            if (HasSnapshotChanged(_lastSnapshot, current))
            {
                RecordSnapshotMilestones(_lastSnapshot, current);

                // Detect and suppress repetitive flapping (same field oscillating)
                if (IsFlapping(_lastSnapshot, current))
                {
                    _flappingCount++;
                    if (_flappingCount <= 3 || _flappingCount % 10 == 0)
                    {
                        // Log first 3 occurrences and then every 10th
                        _diagnostics.LogMemoryChange(_lastSnapshot, current);
                        LogSnapshotChanges(_lastSnapshot, current);
                    }
                }
                else
                {
                    // Flush any accumulated flapping count
                    if (_flappingCount > 3)
                    {
                        _diagnostics.LogInfo("ClientMemory",
                            $"[Memory] (suppressed {_flappingCount - 3} repetitive field changes)");
                    }
                    _flappingCount = 0;
                    _flappingFields.Clear();

                    _diagnostics.LogMemoryChange(_lastSnapshot, current);
                    LogSnapshotChanges(_lastSnapshot, current);
                }

                _lastSnapshot = current;
            }
        }
        catch (Exception ex)
        {
            _diagnostics.LogError("ClientMemory", $"Watch tick error: {ex.Message}");
        }
    }

    private int _flappingCount;
    private readonly HashSet<string> _flappingFields = [];

    /// <summary>
    /// Detects if a memory change is just the same fields oscillating (flapping).
    /// This commonly happens with AccountId toggling during state transitions.
    /// </summary>
    private bool IsFlapping(MemorySnapshot? prev, MemorySnapshot current)
    {
        if (prev == null) return false;

        var p = prev;
        var changedFields = new HashSet<string>();

        if (p.AccountId != current.AccountId) changedFields.Add("AccountId");
        if (p.WorldId != current.WorldId) changedFields.Add("WorldId");
        if (p.ChannelId != current.ChannelId) changedFields.Add("ChannelId");
        if (p.CharacterCount != current.CharacterCount) changedFields.Add("CharacterCount");
        if (p.SlotCount != current.SlotCount) changedFields.Add("SlotCount");
        if (p.RequestSent != current.RequestSent) changedFields.Add("RequestSent");
        if (p.LoginStep != current.LoginStep) changedFields.Add("LoginStep");
        if (p.StepChanging != current.StepChanging) changedFields.Add("StepChanging");
        if (p.CharSelected != current.CharSelected) changedFields.Add("CharSelected");
        if (p.ChannelSelectExists != current.ChannelSelectExists) changedFields.Add("CUIChannelSelect");
        if (p.WorldSelectExists != current.WorldSelectExists) changedFields.Add("CUIWorldSelect");

        if (_flappingFields.Count == 0)
        {
            // First change - record which fields changed
            _flappingFields.UnionWith(changedFields);
            return false;
        }

        // If the exact same fields are changing, it's likely flapping
        return changedFields.SetEquals(_flappingFields);
    }

    private void StopWatch()
    {
        _watchTimer?.Dispose();
        _watchTimer = null;

        var durationMs = (DateTime.UtcNow - _watchStartTime).TotalMilliseconds;
        _logger?.LogInformation("[Diag] Memory watch stopped after {Duration}ms", durationMs);

        // Auto-save the timeline when watch completes
        if (_config.DiagnosticsEnabled)
        {
            SaveTimelineForMode();

            var watchMode = _config.ClientMemory?.WatchMode ?? RueConfigClientMemory.WatchModeActive;
            var report = _diagnostics.GenerateReport();
            var prefix = watchMode.Equals(RueConfigClientMemory.WatchModePassive, StringComparison.OrdinalIgnoreCase)
                ? CompleteReportPrefixManual
                : CompleteReportPrefixAuto;
            var path = LoginDiagnostics.SaveReportToFile(report, prefix);
            _logger?.LogInformation("[Diag] Watch complete report saved to: {Path}", path);

            // If both timelines exist after this capture, generate comparison
            if (LoginDiagnostics.HasBothTimelines)
            {
                _logger?.LogInformation("[Diag] MANUAL vs AUTO COMPARISON available - both timelines captured");

                var comparisonPath = LoginDiagnostics.SaveComparisonReport();
                _logger?.LogInformation("[Diag] Comparison report saved to: {Path}", comparisonPath);
            }
        }
    }

    private static bool HasSnapshotChanged(MemorySnapshot? prev, MemorySnapshot current)
    {
        if (prev == null)
            return true;

        var p = prev;
        return p.AccountId != current.AccountId
            || p.WorldId != current.WorldId
            || p.ChannelId != current.ChannelId
            || p.CharacterCount != current.CharacterCount
            || p.SlotCount != current.SlotCount
            || p.ChannelNameArrayPtr != current.ChannelNameArrayPtr
            || p.AdultChannelArrayPtr != current.AdultChannelArrayPtr
            || p.RequestSent != current.RequestSent
            || p.LoginStep != current.LoginStep
            || p.StepChanging != current.StepChanging
            || p.CharSelected != current.CharSelected
            || p.LoginOpt != current.LoginOpt
            || p.ChannelSelectExists != current.ChannelSelectExists
            || p.SelectedChannel != current.SelectedChannel
            || p.WorldItemPtr != current.WorldItemPtr
            || p.ConnectionDlgExists != current.ConnectionDlgExists
            || p.WorldSelectExists != current.WorldSelectExists
            || p.WorldIdx != current.WorldIdx;
    }

    private void LogSnapshotToLogger(string label, MemorySnapshot s)
    {
        _logger?.LogInformation(
            "[Diag] {Label} snapshot:\n" +
            "  CWvsContext: AcctId={AcctId}, World={World}, Ch={Ch}, CharCount={CharCount}, Slots={Slots}, ChanNamePtr={ChanNamePtr}, AdultChanPtr={AdultChanPtr}\n" +
            "  CLogin: ReqSent={ReqSent}, Step={Step}, StepChanging={StepChg}, CharSel={CharSel}, LoginOpt={LoginOpt}\n" +
            "  CUIChannelSelect: exists={CSExists}, selected={CSSel}, worldItemPtr={WorldItemPtr}, connDlg={ConnDlg}\n" +
            "  CUIWorldSelect: exists={WSExists}, worldIdx={WSIdx}",
            label,
            s.AccountId,
            s.WorldId,
            s.ChannelId,
            s.CharacterCount,
            s.SlotCount,
            s.ChannelNameArrayPtr,
            s.AdultChannelArrayPtr,
            s.RequestSent,
            s.LoginStep,
            s.StepChanging,
            s.CharSelected,
            s.LoginOpt,
            s.ChannelSelectExists,
            s.SelectedChannel,
            s.WorldItemPtr,
            s.ConnectionDlgExists,
            s.WorldSelectExists,
            s.WorldIdx
        );
    }

    private void LogSnapshotChanges(MemorySnapshot? prev, MemorySnapshot current)
    {
        if (prev == null)
        {
            LogSnapshotToLogger("New", current);
            return;
        }

        var p = prev;
        var changes = new List<string>();

        if (p.AccountId != current.AccountId) changes.Add($"AccountId: {p.AccountId}->{current.AccountId}");
        if (p.WorldId != current.WorldId) changes.Add($"WorldId: {p.WorldId}->{current.WorldId}");
        if (p.ChannelId != current.ChannelId) changes.Add($"ChannelId: {p.ChannelId}->{current.ChannelId}");
        if (p.CharacterCount != current.CharacterCount) changes.Add($"CharCount: {p.CharacterCount}->{current.CharacterCount}");
        if (p.SlotCount != current.SlotCount) changes.Add($"SlotCount: {p.SlotCount}->{current.SlotCount}");
        if (p.ChannelNameArrayPtr != current.ChannelNameArrayPtr) changes.Add($"ChannelNamePtr: {p.ChannelNameArrayPtr}->{current.ChannelNameArrayPtr}");
        if (p.AdultChannelArrayPtr != current.AdultChannelArrayPtr) changes.Add($"AdultChannelPtr: {p.AdultChannelArrayPtr}->{current.AdultChannelArrayPtr}");
        if (p.RequestSent != current.RequestSent) changes.Add($"RequestSent: {p.RequestSent}->{current.RequestSent}");
        if (p.LoginStep != current.LoginStep) changes.Add($"LoginStep: {p.LoginStep}->{current.LoginStep}");
        if (p.StepChanging != current.StepChanging) changes.Add($"StepChanging: {p.StepChanging}->{current.StepChanging}");
        if (p.CharSelected != current.CharSelected) changes.Add($"CharSelected: {p.CharSelected}->{current.CharSelected}");
        if (p.LoginOpt != current.LoginOpt) changes.Add($"LoginOpt: {p.LoginOpt}->{current.LoginOpt}");
        if (p.ChannelSelectExists != current.ChannelSelectExists) changes.Add($"CUIChannelSelect: {p.ChannelSelectExists}->{current.ChannelSelectExists}");
        if (p.SelectedChannel != current.SelectedChannel) changes.Add($"SelectedChannel: {p.SelectedChannel}->{current.SelectedChannel}");
        if (p.WorldItemPtr != current.WorldItemPtr) changes.Add($"WorldItemPtr: {p.WorldItemPtr}->{current.WorldItemPtr}");
        if (p.ConnectionDlgExists != current.ConnectionDlgExists) changes.Add($"ConnectionDlg: {p.ConnectionDlgExists}->{current.ConnectionDlgExists}");
        if (p.WorldSelectExists != current.WorldSelectExists) changes.Add($"CUIWorldSelect: {p.WorldSelectExists}->{current.WorldSelectExists}");
        if (p.WorldIdx != current.WorldIdx) changes.Add($"WorldIdx: {p.WorldIdx}->{current.WorldIdx}");

        _logger?.LogInformation(
            "[Diag] Memory changed: {Changes}",
            string.Join(", ", changes)
        );
    }

}
