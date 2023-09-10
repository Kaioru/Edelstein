using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat;

public record SkillContextMobTemporaryStat(
    MobTemporaryStatType Type,
    int Value,
    int Reason,
    DateTime Expire
);
