using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record CheckPasswordResultPacket() : StructuredSendPacket(PacketSendOperation.CheckPasswordResult)
{
    [FieldOrder(0)] public required LoginResultCode Result { get; init; }
    
    [FieldOrder(1)] public byte Unk1 { get; init; } = 0; // nRegStatID
    [FieldOrder(2)] public int Unk2 { get; init; } = 0; // nUseDay
    
    [FieldOrder(3)]
    [SerializeWhen(nameof(Result), LoginResultCode.Blocked)]
    public LoginBlockReasonFrame? BlockReason { get; init; }
    
    [FieldOrder(4)]
    [SerializeWhen(nameof(Result), LoginResultCode.Success)]
    public LoginAccountInfoFrame? AccountInfo { get; init; }
}
