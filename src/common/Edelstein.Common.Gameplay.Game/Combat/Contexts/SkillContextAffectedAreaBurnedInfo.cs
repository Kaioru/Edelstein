using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextAffectedAreaBurnedInfo(
    int SkillID,
    int SkillLevel,
    TimeSpan Interval, 
    TimeSpan Duration,
    IFieldUser User
);
