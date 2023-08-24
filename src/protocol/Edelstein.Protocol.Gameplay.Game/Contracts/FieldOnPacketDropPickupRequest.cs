using Edelstein.Protocol.Gameplay.Game.Objects.Drop;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketDropPickupRequest(
    IFieldUser User,
    IFieldDrop Drop,
    IPoint2D Position
);
