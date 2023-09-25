using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserPortalScriptRequest(
    IFieldUser User,
    string Portal,
    IPoint2D Position
);
