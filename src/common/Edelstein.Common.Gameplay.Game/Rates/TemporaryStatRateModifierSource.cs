using Edelstein.Protocol.Gameplay.Game.Rates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Rates;

public sealed class TemporaryStatRateModifierSource : IRateModifierSource
{
    public ValueTask<IReadOnlyList<IRateModifier>> GetModifiersAsync(RateType type, IRateContext context)
    {
        var user = context.User;
        if (user?.Character.TemporaryStats is not { } stats)
            return ValueTask.FromResult<IReadOnlyList<IRateModifier>>([]);

        IReadOnlyList<IRateModifier> result = [];

        RateModifierBuilder.TryAddPercent(ref result, "event-rate", stats[TemporaryStatType.EventRate]?.Value);

        switch (type)
        {
            case RateType.Exp:
                RateModifierBuilder.TryAddPercent(ref result, "exp-buff", stats[TemporaryStatType.ExpBuffRate]?.Value);
                if (stats[TemporaryStatType.HolySymbol] != null)
                    RateModifierBuilder.TryAddPercent(ref result, "holySymbol-exp", stats[TemporaryStatType.HolySymbol]?.Value);
                if (stats[TemporaryStatType.Dice] != null)
                    RateModifierBuilder.TryAddPercent(ref result, "dice-exp", stats.DiceInfo.EXPr);
                break;
            case RateType.Meso:
                RateModifierBuilder.TryAddPercent(ref result, "meso-up", stats[TemporaryStatType.MesoUp]?.Value);
                RateModifierBuilder.TryAddPercent(ref result, "meso-up-by-item", stats[TemporaryStatType.MesoUpByItem]?.Value);
                if (stats[TemporaryStatType.Dice] != null)
                    RateModifierBuilder.TryAddPercent(ref result, "dice-meso", stats.DiceInfo.MESOr);
                break;
            case RateType.Drop:
                RateModifierBuilder.TryAddPercent(ref result, "item-up-by-item", stats[TemporaryStatType.ItemUpByItem]?.Value);
                break;
        }

        return ValueTask.FromResult(result);
    }
}
