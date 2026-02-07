namespace Edelstein.Protocol.Gameplay.Game.Rates;

public interface IRateModifierSource
{
    ValueTask<IReadOnlyList<IRateModifier>> GetModifiersAsync(RateType type, IRateContext context);
}
