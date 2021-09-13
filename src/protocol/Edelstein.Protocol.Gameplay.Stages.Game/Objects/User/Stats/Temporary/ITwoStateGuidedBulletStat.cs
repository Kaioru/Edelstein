namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary
{
    public interface ITwoStateGuidedBulletStat : ITwoStateTemporaryStat
    {
        int MobID { get; }
        int UserID { get; }
    }
}
