using System.IO;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Cash;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserConsumeCashItemUseRequest : StructuredRecvPacket, ICashItemUseInfo
{
    [FieldOrder(0)] public int UpdateTime { get; init; }
    [FieldOrder(1)] public short Pos { get; init; }
    [FieldOrder(2)] public required UserConsumeCashItemUseRequestInfoEx InfoEx { get; set; }
}

public record UserConsumeCashItemUseRequestInfoEx : StructuredBasePacket, IBinarySerializable
{
    [Ignore] public int TemplateID { get; set; }
    [Ignore] public ICashItemUseInfoEx? Value { get; set; }

    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
    }

    public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var reader = new BinaryReader(stream);
        var serializer = new BinarySerializer();

        TemplateID = reader.ReadInt32();

        Value = TemplateID.GetCashItemType() switch
        {
            CashItemType.AdBoard => serializer.Deserialize<StructuredAdBoardCashItemUseInfoEx>(stream),
            CashItemType.ItemUnrelease => serializer.Deserialize<StructuredItemUnreleaseCashItemUseInfoEx>(stream),
            _ => serializer.Deserialize<StructuredCashItemUseInfoEx>(stream)
        };
    }
}

public record StructuredCashItemUseInfoEx : ICashItemUseInfoEx;
