using BinarySerialization;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users.Messages;

public record StructuredMessageInfoSystemMessage : StructuredMessageInfo
{
    [FieldOrder(0)]
    public required LPString Chat { get; init; }
}
