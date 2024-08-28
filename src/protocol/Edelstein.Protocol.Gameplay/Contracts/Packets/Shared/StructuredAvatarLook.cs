using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts.Packets.Shared;

public record StructuredAvatarLook : StructuredBasePacket
{
    [FieldOrder(0)] public byte Gender { get; init; }
    [FieldOrder(1)] public byte Skin { get; init; }
    [FieldOrder(2)] public int Face { get; init; }
    [FieldOrder(3)] public byte Unk1 { get; init; }
    [FieldOrder(4)] public int Hair { get; init; }

    [FieldOrder(5)]
    [SerializeUntil((byte)0xFF)]
    public List<StructuredAvatarLookEquip> HairEquip { get; init; } = new();
    
    [FieldOrder(6)]
    [SerializeUntil((byte)0xFF)]
    public List<StructuredAvatarLookEquip> UnseenEquip { get; init; } = new();
    
    [FieldOrder(7)]
    public int WeaponStickerID { get; init; }

    [FieldOrder(8)] 
    [FieldCount(0x3)] 
    public int[] PetID { get; init; } = {0, 0, 0};
}

public record StructuredAvatarLookEquip : StructuredBasePacket
{
    [FieldOrder(0)]
    public required byte BodyPart { get; init; }
    
    [FieldOrder(1)]
    public required int ItemID { get; init; }
}
