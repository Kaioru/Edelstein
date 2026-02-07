namespace Edelstein.Plugin.Rue.Diagnostics;

/// <summary>
/// Represents a complete login timeline that can be saved and compared.
/// </summary>
public record LoginTimeline(
    string Mode,
    DateTime CaptureTime,
    TimeSpan TotalDuration,
    DiagnosticEvent[] Events,
    MemorySnapshot? FinalState,
    Dictionary<string, DateTime> Milestones)
{
    public static LoginTimeline Capture(LoginDiagnostics diagnostics, string mode)
    {
        return new LoginTimeline(
            mode,
            DateTime.UtcNow,
            DateTime.UtcNow - diagnostics.StartTime,
            [.. diagnostics.GetEvents()],
            diagnostics.GetLastSnapshot(),
            new Dictionary<string, DateTime>(diagnostics.FlowMilestones));
    }
}
