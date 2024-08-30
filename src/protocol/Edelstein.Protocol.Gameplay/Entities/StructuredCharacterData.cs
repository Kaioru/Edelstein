using System.IO;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities;

public record StructuredCharacterData : StructuredBasePacket, IBinarySerializable
{
    [Ignore] public byte CombatOrders { get; set; }
    [Ignore] public byte Unk1 { get; set; }
    
    [Ignore] public StructuredCharacterDataInfoCharacter? Character { get; set; }
    
    [Ignore] public int? Money { get; set; }
    
    [Ignore] public StructuredCharacterDataInfoInventorySize? InventorySize { get; set; }
    
    [Ignore] public StructuredCharacterDataInfoItemSlotEquips? ItemSlotEquip { get; set; }
    [Ignore] public StructuredCharacterDataInfoItemSlotBundles? ItemSlotConsume { get; set; }
    [Ignore] public StructuredCharacterDataInfoItemSlotBundles? ItemSlotInstall { get; set; }
    [Ignore] public StructuredCharacterDataInfoItemSlotBundles? ItemSlotEtc { get; set; }
    [Ignore] public StructuredCharacterDataInfoItemSlotBundles? ItemSlotCash { get; set; }

    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var writer = new BinaryWriter(stream);
        var flags =
            (Character != null ? StructuredCharacterDataFlag.Character : 0) |
            (Money.HasValue ? StructuredCharacterDataFlag.Money : 0) |
            (InventorySize != null ? StructuredCharacterDataFlag.InventorySize : 0) |
            (ItemSlotEquip != null ? StructuredCharacterDataFlag.ItemSlotEquip : 0) |
            (ItemSlotConsume != null ? StructuredCharacterDataFlag.ItemSlotConsume : 0) |
            (ItemSlotInstall != null ? StructuredCharacterDataFlag.ItemSlotInstall : 0) |
            (ItemSlotEtc != null ? StructuredCharacterDataFlag.ItemSlotEtc : 0) |
            (ItemSlotCash != null ? StructuredCharacterDataFlag.ItemSlotCash : 0);
        
        writer.Write((long)flags);
        writer.Write(CombatOrders);
        writer.Write(Unk1);

        Character?.DispatchTo(stream);
        
        if (Money.HasValue)
            writer.Write(Money.Value);

        InventorySize?.DispatchTo(stream);
        
        ItemSlotEquip?.DispatchTo(stream);
        ItemSlotConsume?.DispatchTo(stream);
        ItemSlotInstall?.DispatchTo(stream);
        ItemSlotEtc?.DispatchTo(stream);
        ItemSlotCash?.DispatchTo(stream);
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

        if (flags.HasFlag(StructuredCharacterDataFlag.Money))
            Money = reader.ReadInt32();

        if (flags.HasFlag(StructuredCharacterDataFlag.InventorySize))
            InventorySize = serializer.Deserialize<StructuredCharacterDataInfoInventorySize>(stream);
        
        if (flags.HasFlag(StructuredCharacterDataFlag.ItemSlotEquip))
            ItemSlotEquip = serializer.Deserialize<StructuredCharacterDataInfoItemSlotEquips>(stream);
        if (flags.HasFlag(StructuredCharacterDataFlag.ItemSlotConsume))
            ItemSlotConsume = serializer.Deserialize<StructuredCharacterDataInfoItemSlotBundles>(stream);
        if (flags.HasFlag(StructuredCharacterDataFlag.ItemSlotInstall))
            ItemSlotInstall = serializer.Deserialize<StructuredCharacterDataInfoItemSlotBundles>(stream);
        if (flags.HasFlag(StructuredCharacterDataFlag.ItemSlotEtc))
            ItemSlotEtc = serializer.Deserialize<StructuredCharacterDataInfoItemSlotBundles>(stream);
        if (flags.HasFlag(StructuredCharacterDataFlag.ItemSlotCash))
            ItemSlotCash = serializer.Deserialize<StructuredCharacterDataInfoItemSlotBundles>(stream);
    }
}
