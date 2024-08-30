using System.IO;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities;

public record StructuredCharacterData : StructuredBasePacket, IBinarySerializable
{
    [Ignore]
    public byte CombatOrders { get; set; }
    
    [Ignore]
    public byte Unk1 { get; set; }
    
    [Ignore]
    public StructuredCharacterDataInfoCharacter? Character { get; set; }

    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var writer = new BinaryWriter(stream);
        var flags = 0 |
                    (Character != null ? StructuredCharacterDataFlag.Character : 0);
        
        writer.Write((long)flags);
        writer.Write(CombatOrders);
        writer.Write(Unk1);

        Character?.DispatchTo(stream);
    }
    public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var reader = new BinaryReader(stream);
        var serializer = new BinarySerializer();
        var flags = (StructuredCharacterDataFlag)reader.ReadInt64();

        CombatOrders = reader.ReadByte();
        Unk1 = reader.ReadByte();

        if (flags.HasFlag(StructuredCharacterDataFlag.Character))
            Character = serializer.Deserialize<StructuredCharacterDataInfoCharacter>(stream);
    }
}
