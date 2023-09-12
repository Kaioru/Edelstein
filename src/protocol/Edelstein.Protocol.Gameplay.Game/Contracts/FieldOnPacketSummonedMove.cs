using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketSummonedMove(
    IFieldUser User,
    IFieldSummoned Summoned,
    IFieldSummonedMovePath Path
);
