namespace Edelstein.Protocol.Gameplay.Game.Combat.Damage;

public interface IDamage
{
    int Value { get; }
    bool IsCritical { get; }
}
