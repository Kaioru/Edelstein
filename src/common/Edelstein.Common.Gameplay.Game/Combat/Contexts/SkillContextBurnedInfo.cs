namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextBurnedInfo(
    int Damage, 
    int SkillID,
    TimeSpan Interval, 
    DateTime Expire
);
