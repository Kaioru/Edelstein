using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Quest;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests
{
    public abstract class AbstractQuestRecordMessage : AbstractQuestMessage
    {
        public override MessageType Type => MessageType.QuestRecordMessage;
        public abstract QuestState State { get; }
        
        public AbstractQuestRecordMessage(short templateID) : base(templateID)
        {
        }

        protected override void EncodeData(IPacket packet)
        {
            base.EncodeData(packet);
            packet.Encode<byte>((byte) State);
        }
    }
}