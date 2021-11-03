namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public interface IMobBurnedStatInfo
    {
        int CharacterID { get; }
        int SkillID { get; }

        int Damage { get; }

        int Interval { get; }
        int End { get; }

        int DotCount { get; }
    }
}
