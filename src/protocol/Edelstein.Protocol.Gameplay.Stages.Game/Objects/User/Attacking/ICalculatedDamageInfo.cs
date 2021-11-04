namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking
{
    public interface ICalculatedDamageInfo
    {
        int Damage { get; }

        bool IsCritical { get; }
    }
}
