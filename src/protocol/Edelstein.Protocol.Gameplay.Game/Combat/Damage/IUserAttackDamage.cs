namespace Edelstein.Protocol.Gameplay.Game.Combat.Damage;

public interface IUserAttackDamage
{
    int Damage { get; }
    bool IsCritical { get; }
}
