namespace Edelstein.Plugin.Rue.Configs;

/// <summary>
/// Configuration for auto-created characters when none exist during auto-login.
/// </summary>
public record RueConfigAutoCharacter
{
    public string NamePrefix { get; set; } = "Auto";
    public int Race { get; set; } = 1;
    public short SubJob { get; set; } = 0;
    public int Face { get; set; } = 20000;
    public int Hair { get; set; } = 30000;
    public int HairColor { get; set; } = 0;
    public int Skin { get; set; } = 0;
    public int Coat { get; set; } = 1040000;
    public int Pants { get; set; } = 1060000;
    public int Shoes { get; set; } = 1072000;
    public int Weapon { get; set; } = 1302000;
    public byte Gender { get; set; } = 0;
}
