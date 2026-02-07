using Edelstein.Protocol.Gameplay.Game.Rates;

namespace Edelstein.Common.Gameplay.Game.Rates;

public sealed class RateModifierManager : IRateModifierManager
{
    private readonly IRateModifierSource[] _sources;

    public RateModifierManager(IEnumerable<IRateModifierSource> sources) =>
        _sources = [.. sources];

    // Sync versions - for use in synchronous contexts (e.g., scripting)
    public IReadOnlyList<IRateModifier> GetModifiers(RateType type, IRateContext context) =>
        GetModifiersAsync(type, context).AsTask().GetAwaiter().GetResult();

    public double GetFinalRate(RateType type, IRateContext context) =>
        GetFinalRateAsync(type, context).AsTask().GetAwaiter().GetResult();

    public async ValueTask<IReadOnlyList<IRateModifier>> GetModifiersAsync(RateType type, IRateContext context)
    {
        List<IRateModifier>? modifiers = null;

        foreach (var source in _sources)
        {
            foreach (var modifier in await source.GetModifiersAsync(type, context))
            {
                if (modifier.Multiplier <= 0d) continue;
                modifiers ??= [];
                modifiers.Add(modifier);
            }
        }

        if (modifiers is not { Count: > 0 }) return [];
        if (modifiers.Count > 1)
            modifiers.Sort((x, y) => (x.Priority ?? 0).CompareTo(y.Priority ?? 0));
        return modifiers;
    }

    public async ValueTask<double> GetFinalRateAsync(RateType type, IRateContext context)
    {
        var rate = 1d;

        foreach (var source in _sources)
        {
            foreach (var modifier in await source.GetModifiersAsync(type, context))
            {
                if (modifier.Multiplier <= 0d)
                    return 0d;
                rate *= modifier.Multiplier;
            }
        }

        return rate;
    }
}
