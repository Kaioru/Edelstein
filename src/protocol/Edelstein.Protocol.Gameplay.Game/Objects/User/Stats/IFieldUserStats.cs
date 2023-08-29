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

    int ACC { get; }
    int EVA { get; }

    int Craft { get; }
    int Speed { get; }
    int Jump { get; }
    
    int STRr { get; }
    int DEXr { get; }
    int INTr { get; }
    int LUKr { get; }
    int MaxHPr { get; }
    int MaxMPr { get; }
}
