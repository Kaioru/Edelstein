using Edelstein.Protocol.Gameplay.Game.Objects.Dragon;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketDragonMove(
    IFieldUser User,
    IFieldDragon Dragon,
    IFieldDragonMovePath Path
);
