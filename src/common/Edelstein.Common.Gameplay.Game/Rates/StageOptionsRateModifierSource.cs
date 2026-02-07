using Edelstein.Protocol.Gameplay.Game.Rates;

namespace Edelstein.Common.Gameplay.Game.Rates;

public sealed class StageOptionsRateModifierSource : IRateModifierSource
{
    public ValueTask<IReadOnlyList<IRateModifier>> GetModifiersAsync(RateType type, IRateContext context)
    {
        if (context.Options is not { } options)
            return ValueTask.FromResult<IReadOnlyList<IRateModifier>>([]);

        var (name, rate) = type switch
        {
            RateType.Exp => ("stage-exp", options.ExpRate),
            RateType.Meso => ("stage-meso", options.MesoRate),
            RateType.Drop => ("stage-drop", options.DropRate),
            _ => (null, 1.0)
        };

        return ValueTask.FromResult(name != null && rate != 1.0
            ? RateModifierBuilder.Single(name, rate)
            : []);
    }
}
