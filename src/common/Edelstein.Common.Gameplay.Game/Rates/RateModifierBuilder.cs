using Edelstein.Protocol.Gameplay.Game.Rates;

namespace Edelstein.Common.Gameplay.Game.Rates;

internal static class RateModifierBuilder
{
    public static bool TryAddPercent(ref IReadOnlyList<IRateModifier> result, string source, int? value)
    {
        if (value is not > 0) return false;

        var multiplier = 1d + value.Value / 100d;
        if (multiplier <= 0d) return false;

        var modifier = new RateModifier(source, multiplier, null);
        result = result.Count == 0
            ? [modifier]
            : [..result, modifier];
        return true;
    }

    public static IReadOnlyList<IRateModifier> Single(string source, double multiplier, int? priority = null) =>
        multiplier > 0d ? [new RateModifier(source, multiplier, priority)] : [];
}
