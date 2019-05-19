using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests
{
    public class QuestRecordExMessage : AbstractQuestMessage
    {
        public override MessageType Type => MessageType.QuestRecordExMessage;
        private readonly string _value;

        public QuestRecordExMessage(short templateID, string value) : base(templateID)
        {
            _value = value;
        }

        protected override void EncodeData(IPacket packet)
        {
            base.EncodeData(packet);
            packet.Encode<string>(_value);
        }
    }
}