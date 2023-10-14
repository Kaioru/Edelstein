using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public class FieldUserStatsForced : IFieldUserStatsForced
{
    public short? STR { get; set; }
    public short? DEX { get; set; }
    public short? INT { get; set; }
    public short? LUK { get; set; }
    
    public short? PAD { get; set; }
    public short? PDD { get; set; }
    public short? MAD { get; set; }
    public short? MDD { get; set; }
    public short? EVA { get; set; }
    public short? ACC { get; set; }
    
    public byte? Speed { get; set; }
    public byte? Jump { get; set; }
    
    public byte? SpeedMax { get; set; }
}
