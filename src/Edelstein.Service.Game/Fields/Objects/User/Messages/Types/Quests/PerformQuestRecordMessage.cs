using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Quest;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests
{
    public class PerformQuestRecordMessage : AbstractQuestRecordMessage
    {
        public override QuestState State => QuestState.Perform;
        private readonly string _value;
        
        public PerformQuestRecordMessage(short templateID, string value) : base(templateID)
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