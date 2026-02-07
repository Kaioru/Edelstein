namespace Edelstein.Plugin.Rue.Configs;

/// <summary>
/// Configuration for Rue game plugin.
/// </summary>
public record RueConfigGame
{
    /// <summary>Enables detailed logging of trie indexing statistics.</summary>
    public bool LogTrieTelemetry { get; set; }
}
