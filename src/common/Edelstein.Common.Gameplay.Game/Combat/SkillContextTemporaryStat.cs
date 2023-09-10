using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat;

public record SkillContextTemporaryStat(
    TemporaryStatType Type,
    int Value,
    int Reason,
    DateTime Expire
);
