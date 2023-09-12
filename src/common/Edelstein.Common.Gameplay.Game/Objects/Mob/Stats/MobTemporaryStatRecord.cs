using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

public record MobTemporaryStatRecord : IMobTemporaryStatRecord
{
    public int Value { get; set; }
    public int Reason { get; set; }
    
    public DateTime? DateExpire { get; set; }
}
