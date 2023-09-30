namespace Edelstein.Plugin.Rue.Configs;

public record RueConfigLogin
{
    public bool IsAutoRegister { get; set; }
    public bool IsAutoLogin { get; set; }
    public bool IsFlippedUsername { get; set; }
    
    public RueConfigLoginCredentials? LoginCredentials { get; set; }
}
