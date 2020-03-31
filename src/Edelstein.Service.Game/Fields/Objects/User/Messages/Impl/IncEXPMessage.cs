using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl
{
    public class IncEXPMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.IncEXPMessage;

        public bool IsLastHit { get; set; }
        public int EXP { get; set; }

        public bool OnQuest { get; set; }
        public int EXPBySMQ { get; set; }

        public byte MobEventBonusPercentage { get; set; }
        public byte PartyBonusPercentage { get; set; }

        public int WeddingBonusEXP { get; set; }

        public byte PlayTimeHour { get; set; }

        public byte QuestBonusRate { get; set; }
        public byte QuestBonusRemainCount { get; set; }

        public byte PartyBonusEventRate { get; set; }
        public int PartyBonusEXP { get; set; }

        public int ItemBonusEXP { get; set; }
        public int PremiumIPEXP { get; set; }
        public int RainbowWeekEventEXP { get; set; }
        public int PartyEXPRingEXP { get; set; }
        public int CakePieEventBonus { get; set; }

        protected override void EncodeData(IPacket packet)
        {
            packet.EncodeBool(IsLastHit);
            packet.EncodeInt(EXP);
            packet.EncodeBool(OnQuest);
            packet.EncodeInt(EXPBySMQ);

            packet.EncodeByte(MobEventBonusPercentage);
            packet.EncodeByte(PartyBonusPercentage);

            packet.EncodeInt(WeddingBonusEXP);

            if (MobEventBonusPercentage > 0)
                packet.EncodeByte(PlayTimeHour);

            if (OnQuest)
            {
                packet.EncodeByte(QuestBonusRate);
                if (QuestBonusRate > 0)
                    packet.EncodeByte(QuestBonusRemainCount);
            }

            packet.EncodeByte(PartyBonusEventRate);
            packet.EncodeInt(PartyBonusEXP);

            packet.EncodeInt(ItemBonusEXP);
            packet.EncodeInt(PremiumIPEXP);
            packet.EncodeInt(RainbowWeekEventEXP);
            packet.EncodeInt(PartyEXPRingEXP);
            packet.EncodeInt(CakePieEventBonus);
        }
    }
}