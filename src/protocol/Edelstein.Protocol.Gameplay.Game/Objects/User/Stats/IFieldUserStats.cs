namespace Edelstein.Protocol.Gameplay.Game.Objects.User.Stats;

public interface IFieldUserStats
{
    int STR { get; }
    int DEX { get; }
    int INT { get; }
    int LUK { get; }
    
    int MaxHP { get; }
    int MaxMP { get; }
    
    int PAD { get; }
    int PDD { get; }
    int MAD { get; }
    int MDD { get; }

    int Craft { get; }
    int Speed { get; }
    int Jump { get; }
}
