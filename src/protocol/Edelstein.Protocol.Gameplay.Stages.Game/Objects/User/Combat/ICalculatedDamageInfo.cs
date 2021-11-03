namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Combat
{
    public interface ICalculatedDamageInfo
    {
        int Damage { get; }
        bool Critical { get; }
    }
}
