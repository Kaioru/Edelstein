namespace Edelstein.Common.Gameplay.Models.Characters.Stats.TwoState;

public record TwoStateEnergyChargedRecord : TwoStateTemporaryStatRecordDynamicTerm
{
    public override bool IsActive() => Value >= 10000;
}
