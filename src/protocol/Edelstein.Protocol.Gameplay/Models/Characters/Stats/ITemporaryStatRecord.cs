namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats;

public interface ITemporaryStatRecord
{
    int Value { get; set; }
    int Reason { get; set; }

    DateTime? DateExpire { get; set; }
}
