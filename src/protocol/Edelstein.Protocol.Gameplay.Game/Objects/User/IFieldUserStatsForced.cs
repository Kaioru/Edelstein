namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUserStatsForced
{
    short? STR { get; set; }
    short? DEX { get; set; }
    short? INT { get; set; }
    short? LUK { get; set; }
    
    short? PAD { get; set; }
    short? PDD { get; set; }
    short? MAD { get; set; }
    short? MDD { get; set; }
    short? EVA { get; set; }
    short? ACC { get; set; }
    
    byte? Speed { get; set; }
    byte? Jump { get; set; }
    
    byte? SpeedMax { get; set; }
}
