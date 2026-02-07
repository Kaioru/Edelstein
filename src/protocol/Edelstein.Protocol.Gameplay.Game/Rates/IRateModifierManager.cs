namespace Edelstein.Protocol.Gameplay.Game.Rates;

public interface IRateModifierManager
{
    IReadOnlyList<IRateModifier> GetModifiers(RateType type, IRateContext context);
    double GetFinalRate(RateType type, IRateContext context);

    ValueTask<IReadOnlyList<IRateModifier>> GetModifiersAsync(RateType type, IRateContext context);
    ValueTask<double> GetFinalRateAsync(RateType type, IRateContext context);
}
