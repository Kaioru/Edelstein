using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Quest;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests
{
    public class ResignQuestRecordMessage : AbstractQuestRecordMessage
    {
        public override QuestState State => QuestState.None;
        
        public ResignQuestRecordMessage(short templateID) : base(templateID)
        {
        }

        protected override void EncodeData(IPacket packet)
        {
            base.EncodeData(packet);
            packet.Encode<bool>(false); // TODO
        }
    }
}