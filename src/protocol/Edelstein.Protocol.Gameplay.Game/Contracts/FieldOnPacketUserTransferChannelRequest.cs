using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserTransferChannelRequest(
    IFieldUser User,
    byte ChannelID
);
