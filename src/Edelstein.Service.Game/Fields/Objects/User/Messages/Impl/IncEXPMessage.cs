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
            packet.Encode<bool>(IsLastHit);
            packet.Encode<int>(EXP);
            packet.Encode<bool>(OnQuest);
            packet.Encode<int>(EXPBySMQ);

            packet.Encode<byte>(MobEventBonusPercentage);
            packet.Encode<byte>(PartyBonusPercentage);

            packet.Encode<int>(WeddingBonusEXP);

            if (MobEventBonusPercentage > 0)
                packet.Encode<byte>(PlayTimeHour);

            if (OnQuest)
            {
                packet.Encode<byte>(QuestBonusRate);
                if (QuestBonusRate > 0)
                    packet.Encode<byte>(QuestBonusRemainCount);
            }

            packet.Encode<byte>(PartyBonusEventRate);
            packet.Encode<int>(PartyBonusEXP);

            packet.Encode<int>(ItemBonusEXP);
            packet.Encode<int>(PremiumIPEXP);
            packet.Encode<int>(RainbowWeekEventEXP);
            packet.Encode<int>(PartyEXPRingEXP);
            packet.Encode<int>(CakePieEventBonus);
        }
    }
}