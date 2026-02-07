namespace Edelstein.Plugin.Rue.Diagnostics;

/// <summary>
/// Represents a diagnostic event (packet sent/received, memory changed, state transition, etc.)
/// </summary>
public record DiagnosticEvent(
    DateTime Timestamp,
    string Source,
    string EventType,
    string Description,
    Dictionary<string, object?>? Details = null);
