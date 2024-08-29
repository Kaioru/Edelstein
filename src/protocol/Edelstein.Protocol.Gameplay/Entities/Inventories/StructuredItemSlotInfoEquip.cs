using BinarySerialization;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record StructuredItemSlotInfoEquip : StructuredItemSlotInfoBase
{
    [FieldOrder(0)] public byte RUC { get; init; }
    [FieldOrder(1)] public byte CUC { get; init; }

    [FieldOrder(2)] public short STR { get; init; }
    [FieldOrder(3)] public short DEX { get; init; }
    [FieldOrder(4)] public short INT { get; init; }
    [FieldOrder(5)] public short LUK { get; init; }
    [FieldOrder(6)] public short MaxHP { get; init; }
    [FieldOrder(7)] public short MaxMP { get; init; }
    [FieldOrder(8)] public short PAD { get; init; }
    [FieldOrder(9)] public short MAD { get; init; }
    [FieldOrder(10)] public short PDD { get; init; }
    [FieldOrder(11)] public short MDD { get; init; }
    [FieldOrder(12)] public short ACC { get; init; }
    [FieldOrder(13)] public short EVA { get; init; }

    [FieldOrder(14)] public short Craft { get; init; }
    [FieldOrder(15)] public short Speed { get; init; }
    [FieldOrder(16)] public short Jump { get; init; }

    [FieldOrder(17)] public LPString Title { get; init; } = new();
    [FieldOrder(18)] public short Attribute { get; init; }
    [FieldOrder(19)] public byte LevelUpType { get; init; }
    [FieldOrder(20)] public byte Level { get; init; }
    [FieldOrder(21)] public int EXP { get; init; }
    [FieldOrder(22)] public int Durability { get; init; }

    [FieldOrder(23)] public int IUC { get; init; }

    [FieldOrder(24)] public byte Grade { get; init; }
    [FieldOrder(25)] public byte CHUC { get; init; }

    [FieldOrder(26)] public short Option1 { get; init; }
    [FieldOrder(27)] public short Option2 { get; init; }
    [FieldOrder(28)] public short Option3 { get; init; }
    [FieldOrder(29)] public short Socket1 { get; init; }
    [FieldOrder(30)] public short Socket2 { get; init; }
    
    [FieldOrder(31)]
    [SerializeWhen(nameof(CashItemSN), null)]
    public long SN { get; init; }

    [FieldOrder(32)] public FDateTime DateEquipped { get; init; } = new();
    [FieldOrder(33)] public int PrevBonusExpRate { get; init; }
}
