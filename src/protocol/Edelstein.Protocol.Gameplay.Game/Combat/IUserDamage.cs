namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IUserDamage
{
    int Damage { get; }
    bool IsCritical { get; }
}
