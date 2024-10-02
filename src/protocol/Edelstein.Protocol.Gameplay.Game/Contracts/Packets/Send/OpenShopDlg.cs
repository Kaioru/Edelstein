using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record OpenShopDlg() : StructuredSendPacket((short)PacketSendOperation.OpenShopDlg)
{
    [FieldOrder(0)] public required int NPCTemplateID { get; init; }
    
    [FieldOrder(1)] public short Count { get; init; }

    [FieldOrder(2)]
    [FieldCount(nameof(Count))]
    public List<OpenShopDlgItem> Items { get; init; } = new();
}

public record OpenShopDlgItem : StructuredBasePacket
{
    [FieldOrder(0)] public int ItemID { get; init; }
    
    [FieldOrder(1)] public int Price { get; init; }
    [FieldOrder(2)] public byte DiscountRate { get; init; }
    
    [FieldOrder(3)] public int TokenItemID { get; init; }
    [FieldOrder(4)] public int TokenPrice { get; init; }
    
    [FieldOrder(5)] public int ItemPeriod { get; init; }
    [FieldOrder(6)] public int LevelLimited { get; init; }

    [Ignore] public bool IsRechargeableItem => ItemID.IsRechargeableItem();
    
    [FieldOrder(7)] 
    [SerializeWhen(nameof(IsRechargeableItem), false)]
    public short Quantity { get; init; }
    
    [FieldOrder(8)]
    [SerializeWhen(nameof(IsRechargeableItem), true)]
    public double UnitPrice { get; init; }
    
    [FieldOrder(9)] public short MaxPerSlot { get; init; }
}
