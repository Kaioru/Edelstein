using BinarySerialization;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users.Messages;

public record StructuredMessageInfoIncEXP : StructuredMessageInfo
{
    [FieldOrder(0)] public bool Unk1 { get; init; }
    [FieldOrder(1)] public int EXP { get; init; }
    [FieldOrder(2)] public bool OnQuest { get; init; }
    [FieldOrder(3)] public int Unk2 { get; init; }
    [FieldOrder(4)] public byte MobEventBonusPercentage { get; init; }
    [FieldOrder(5)] public byte Unk3 { get; init; }
    [FieldOrder(6)] public int WeddingBonusEXP { get; init; }
    
    [FieldOrder(7)] 
    [SerializeWhen(nameof(MobEventBonusPercentage), 0, ComparisonOperator.GreaterThan)]
    public byte PlayTimeHour { get; init; }
    
    [FieldOrder(8)] 
    [SerializeWhen(nameof(OnQuest), true)]
    public byte SpiritWeekEventEXP { get; init; }
    
    [FieldOrder(9)] 
    [SerializeWhen(nameof(OnQuest), true)]
    [SerializeWhen(nameof(SpiritWeekEventEXP), 0, ComparisonOperator.NotEqual)]
    public byte QuestBonusRemainCount { get; init; }
    
    [FieldOrder(10)] public byte PartyBonusEventRate { get; init; }
    [FieldOrder(11)] public int PartyBonusEXP { get; init; }
    [FieldOrder(12)] public int ItemBonusEXP { get; init; }
    [FieldOrder(13)] public int PremiumIPEXP { get; init; }
    [FieldOrder(14)] public int RainbowWeekEventEXP { get; init; }
    [FieldOrder(15)] public int PartyEXPRingEXP { get; init; }
    [FieldOrder(16)] public int CakePieEventBonus { get; init; }
}
