using Edelstein.Protocol.Gameplay.Game.Rates;

namespace Edelstein.Common.Gameplay.Game.Rates;

public record RateModifier(string Source, double Multiplier, int? Priority) : IRateModifier
{
    public static int Apply(int amount, double rate)
    {
        if (amount <= 0 || rate <= 0) return 0;

        var scaled = amount * rate;
        return scaled >= int.MaxValue
            ? int.MaxValue
            : (int)Math.Round(scaled, MidpointRounding.AwayFromZero);
    }
}
