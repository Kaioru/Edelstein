using System.IO;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserAvatarModified() : StructuredSendPacket((short)PacketSendOperation.UserAvatarModified)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required UserAvatarModifiedInfo Info { get; init; }
}

public record UserAvatarModifiedInfo : StructuredBasePacket, IBinarySerializable
{
    [Ignore] public StructuredCharacterLook? CharacterLook { get; set; }
    [Ignore] public byte? CharacterSpeed { get; set; }
    [Ignore] public byte? CarryItemEffectCount { get; set; }
    
    [Ignore] public bool Couple { get; set; }
    [Ignore] public bool Friendship { get; set; }
    [Ignore] public bool Marriage { get; set; }
    
    [Ignore] public int CompletedSetItemID { get; set; }
    
    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var writer = new BinaryWriter(stream);
        var flags =
            (CharacterLook != null ? 0x1 : 0) |
            (CharacterSpeed != null ? 0x2 : 0) |
            (CarryItemEffectCount != null ? 0x4 : 0);
        
        writer.Write((byte)flags);
        
        CharacterLook?.DispatchTo(stream);
        if (CharacterSpeed.HasValue) writer.Write(CharacterSpeed.Value);
        if (CarryItemEffectCount.HasValue) writer.Write(CarryItemEffectCount.Value);
        
        writer.Write(Couple);
        writer.Write(Friendship);
        writer.Write(Marriage);
        
        writer.Write(CompletedSetItemID);
    }
    
    public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var reader = new BinaryReader(stream);
        var serializer = new BinarySerializer();
        var flags = reader.ReadByte();
        
        if ((flags & 1) != 0)
            CharacterLook = serializer.Deserialize<StructuredCharacterLook>(stream);
        if ((flags & 2) != 0)
            CharacterSpeed = reader.ReadByte();
        if ((flags & 4) != 0)
            CarryItemEffectCount = reader.ReadByte();

        Couple = reader.ReadBoolean();
        Friendship = reader.ReadBoolean();
        Marriage = reader.ReadBoolean();

        CompletedSetItemID = reader.ReadInt32();
    }
}
