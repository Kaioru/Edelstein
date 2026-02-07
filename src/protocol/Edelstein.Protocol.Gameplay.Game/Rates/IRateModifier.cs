namespace Edelstein.Protocol.Gameplay.Game.Rates;

public interface IRateModifier
{
    string Source { get; }
    double Multiplier { get; }
    int? Priority { get; }
}
