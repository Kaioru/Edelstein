namespace Edelstein.Plugin.Rue.Configs;

/// <summary>
/// Configuration for Rue login plugin auto-login and auto-registration features.
/// </summary>
public record RueConfigLogin
{
    public const int DefaultAutoSelectDelayMs = 500;

    public bool DiagnosticsEnabled { get; set; }
    public bool IsAutoRegister { get; set; }
    public bool IsAutoLogin { get; set; }
    public bool IsFlippedUsername { get; set; }

    public RueConfigLoginCredentials? LoginCredentials { get; set; }

    public byte? AutoSelectWorldID { get; set; }
    public byte? AutoSelectChannelID { get; set; }

    public string? AutoSelectCharacterName { get; set; }
    public int? AutoSelectCharacterIndex { get; set; }

    /// <summary>Second password used for auto-login when account has SPW set.</summary>
    public string? AutoSPW { get; set; }

    /// <summary>Delay in milliseconds between auto-login stages to allow client to process responses.</summary>
    public int AutoSelectDelayMs { get; set; } = DefaultAutoSelectDelayMs;

    public bool AutoCreateCharacter { get; set; }
    public RueConfigAutoCharacter? AutoCharacterConfig { get; set; }

    /// <summary>
    /// Client memory modification settings for full auto-login.
    /// Required because CWvsContext->m_nWorldID can only be set client-side.
    /// </summary>
    public RueConfigClientMemory? ClientMemory { get; set; }
}
