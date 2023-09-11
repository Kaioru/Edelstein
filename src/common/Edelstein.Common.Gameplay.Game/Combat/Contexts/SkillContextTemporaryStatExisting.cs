using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextTemporaryStatExisting(
    TemporaryStatType Type,
    int Value
);
