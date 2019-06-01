using Edelstein.Provider.Templates.Quest;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields.Quests
{
    public class QuestSpeaker : Speaker
    {
        public override SpeakerParamType ParamType => 0;
        private readonly FieldUser _fieldUser;
        private readonly short _questTemplateID;

        public QuestSpeaker(
            IConversationContext context,
            FieldUser fieldUser,
            short questTemplateID,
            int npcTemplateID
        ) : base(context, npcTemplateID)
        {
            _fieldUser = fieldUser;
            _questTemplateID = questTemplateID;
        }

        // TODO: redundant code
        public QuestState State => _fieldUser.Character.QuestComplete.ContainsKey(_questTemplateID)
            ? QuestState.Complete
            : _fieldUser.Character.QuestRecord.ContainsKey(_questTemplateID)
                ? QuestState.Perform
                : QuestState.None;

        public void Accept(string value = "")
            => Update(value);

        public void Update(string value)
            => _fieldUser.ModifyQuests(q => q.Update(_questTemplateID, value)).Wait();

        public void UpdateEx(string value)
            => _fieldUser.ModifyQuests(q => q.UpdateEx(_questTemplateID, value)).Wait();

        public void Complete()
            => _fieldUser.ModifyQuests(q => q.Complete(_questTemplateID)).Wait();

        public void Resign()
            => _fieldUser.ModifyQuests(q => q.Resign(_questTemplateID)).Wait();
    }
}