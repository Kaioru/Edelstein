using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextMobTemporaryStat(
    MobTemporaryStatType Type,
    int Value,
    int Reason,
    DateTime Expire
);
