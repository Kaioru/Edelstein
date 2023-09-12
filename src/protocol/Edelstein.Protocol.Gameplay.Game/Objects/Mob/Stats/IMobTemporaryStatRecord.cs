namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

public interface IMobTemporaryStatRecord
{
    int Value { get; set; }
    int Reason { get; set; }

    DateTime? DateExpire { get; set; }
}
