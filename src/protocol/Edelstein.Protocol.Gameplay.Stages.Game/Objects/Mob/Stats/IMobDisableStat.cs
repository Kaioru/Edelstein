namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public interface IMobDisableStat : IMobStat
    {
        bool Invincible { get; }
        bool Disable { get; }
    }
}
