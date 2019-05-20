using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Quest;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests
{
    public class ResignQuestRecordMessage : AbstractQuestRecordMessage
    {
        public override QuestState State => QuestState.None;
        private readonly bool _completed;
        
        public ResignQuestRecordMessage(short templateID, bool completed) : base(templateID)
        {
            _completed = completed;
        }

        protected override void EncodeData(IPacket packet)
        {
            base.EncodeData(packet);
            packet.Encode<bool>(_completed);
        }
    }
}