using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats;

public record FieldUserStats : IFieldUserStats
{
    public int STR { get; init; }
    public int DEX { get; init; }
    public int INT { get; init; }
    public int LUK { get; init; }
    
    public int MaxHP { get; init; }
    public int MaxMP { get; init; }
    
    public int PAD { get; init; }
    public int PDD { get; init; }
    public int MAD { get; init; }
    public int MDD { get; init; }
    
    public int PACC { get; init; }
    public int MACC { get; init; }
    public int PEVA { get; init; }
    public int MEVA { get; init; }

    public int Craft { get; init; }
    public int Speed { get; init; }
    public int Jump { get; init; }
}
