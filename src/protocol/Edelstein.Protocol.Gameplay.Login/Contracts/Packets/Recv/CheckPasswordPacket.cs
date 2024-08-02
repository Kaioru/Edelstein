using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;

public record CheckPasswordPacket : StructuredRecvPacket
{
    [FieldOrder(0)] public required LPString Username { get; init; }
    [FieldOrder(1)] public required LPString Password { get; init; }
    
    [FieldOrder(2)]
    [FieldLength(0x10)]
    public required byte[] MachineID { get; init; }
    
    [FieldOrder(3)] public required int GameRoomClient { get; init; }
    [FieldOrder(4)] public required byte GameStartMode { get; init; }
    [FieldOrder(5)] public required bool Unk1 { get; init; } // bAdminclient (?)
    [FieldOrder(6)] public required bool Unk2 { get; init; } // unsure
    
    [FieldOrder(7)] public required int PartnerCode { get; init; }
}
