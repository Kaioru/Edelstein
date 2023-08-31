namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IUserAttackDamage
{
    int Damage { get; }
    bool IsCritical { get; }
}
