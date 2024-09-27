using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public record StructuredMobInfo : StructuredBasePacket
{
    [FieldOrder(0)] public byte CalcDamageStatIndex { get; init; }
    [FieldOrder(1)] public required int TemplateID { get; init; }
    
    [FieldOrder(2)] public long MobStatFlag1 { get; init; }
    [FieldOrder(3)] public long MobStatFlag2 { get; init; }
    
    [FieldOrder(4)] public short X { get; init; }
    [FieldOrder(5)] public short Y { get; init; }
    [FieldOrder(6)] public byte MoveAction { get; init; }
    [FieldOrder(7)] public short Fh { get; init; }
    [FieldOrder(8)] public short FhStart { get; init; }
    
    [FieldOrder(9)] public FieldMobAppearType AppearType { get; init; }
    [FieldOrder(10)] 
    [SerializeWhen(nameof(AppearType), FieldMobAppearType.Revived)]
    [SerializeWhen(nameof(AppearType), FieldMobAppearType.Normal, ComparisonOperator.GreaterThan)]
    public int AppearOption { get; init; }
    
    [FieldOrder(11)] public byte TeamForMCarnival { get; init; }
    [FieldOrder(12)] public int EffectItemID { get; init; }
    [FieldOrder(13)] public int Phase { get; init; }
}
