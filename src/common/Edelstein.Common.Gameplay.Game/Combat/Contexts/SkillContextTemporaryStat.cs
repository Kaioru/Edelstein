using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextTemporaryStat(
    TemporaryStatType Type,
    int Value,
    int Reason,
    DateTime Expire
);
