using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;

public record MigrateIn : StructuredRecvPacket
{
    [FieldOrder(0)] public required int CharacterID { get; init; }
    
    [FieldOrder(1)]
    [FieldLength(0x10)]
    public required byte[] MachineID { get; init; }
    
    [FieldOrder(2)]
    public required bool IsUserGM { get; init; }
    
    [FieldOrder(3)]
    public required byte Unk1 { get; init; }
    
    [FieldOrder(4)]
    public required long ClientKey { get; init; }
}
