using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserTransferFieldRequest(
    IFieldUser User,
    int FieldID,
    string PortalID
);
