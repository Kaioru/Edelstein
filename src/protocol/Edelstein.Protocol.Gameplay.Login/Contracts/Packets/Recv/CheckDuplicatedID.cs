using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;

public record CheckDuplicatedID : StructuredRecvPacket
{
    [FieldOrder(0)]
    public required LPString CharName { get; init; }
}
