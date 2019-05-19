using System;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Quest;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests
{
    public class CompleteQuestRecordMessage : AbstractQuestRecordMessage
    {
        public override QuestState State => QuestState.Complete;
        private readonly DateTime _dateCompleted;

        public CompleteQuestRecordMessage(short templateID, DateTime dateCompleted) : base(templateID)
        {
            _dateCompleted = dateCompleted;
        }

        protected override void EncodeData(IPacket packet)
        {
            base.EncodeData(packet);
            packet.Encode<DateTime>(_dateCompleted);
        }
    }
}