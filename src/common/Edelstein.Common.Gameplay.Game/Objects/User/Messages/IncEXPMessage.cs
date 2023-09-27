using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record IncEXPMessage(
    int EXP,
    bool OnQuest = false,
    int SelectedMobBonus = 0,
    byte MobEventBonusPercentage = 0,
    int WeddingBonusEXP = 0,
    byte PlayTimeHour = 0,
    byte QuestBonusRate = 0,
    byte QuestBonusRemainCount = 0,
    byte PartyBonusEventRate = 0,
    int PartyBonusEXP = 0,
    int ItemBonusEXP = 0,
    int PremiumIPExp = 0,
    int RainbowWeekEventEXP = 0,
    int PartyEXPRingEXP = 0,
    int CakePieEventBonus = 0
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.IncEXPMessage);
        writer.WriteBool(true);
        writer.WriteInt(EXP);
        writer.WriteBool(OnQuest);
        writer.WriteInt(SelectedMobBonus);
        writer.WriteByte(MobEventBonusPercentage);
        writer.WriteByte(0);
        writer.WriteInt(WeddingBonusEXP);

        if (MobEventBonusPercentage > 0)
            writer.WriteByte(PlayTimeHour);

        if (OnQuest)
        {
            writer.WriteByte(QuestBonusRate);
            writer.WriteByte(QuestBonusRemainCount);
        }

        writer.WriteByte(PartyBonusEventRate);
        writer.WriteInt(PartyBonusEXP);
        writer.WriteInt(ItemBonusEXP);
        writer.WriteInt(PremiumIPExp);
        writer.WriteInt(RainbowWeekEventEXP);
        writer.WriteInt(PartyEXPRingEXP);
        writer.WriteInt(CakePieEventBonus);
    }
}
