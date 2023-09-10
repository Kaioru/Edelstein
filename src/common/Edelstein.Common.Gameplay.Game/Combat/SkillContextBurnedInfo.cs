namespace Edelstein.Common.Gameplay.Game.Combat;

public record SkillContextBurnedInfo(
    int Damage, 
    int SkillID,
    TimeSpan Interval, 
    DateTime Expire
);
