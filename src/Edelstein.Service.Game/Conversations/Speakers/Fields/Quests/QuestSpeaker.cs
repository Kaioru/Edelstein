using Edelstein.Provider.Templates.Quest;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Quests;

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

        public QuestState State => _fieldUser.Character.GetQuestState(_questTemplateID);

        public string Record
        {
            get => _fieldUser.Character.GetQuestRecord(_questTemplateID);
            set => Update(value);
        }

        public string RecordEx
        {
            get => _fieldUser.Character.GetQuestRecordEX(_questTemplateID);
            set => UpdateEx(value);
        }

        public string RecordKey(string key)
            => _fieldUser.Character.GetQuestRecord(_questTemplateID, key);

        public string RecordExKey(string key)
            => _fieldUser.Character.GetQuestRecordEX(_questTemplateID, key);

        public void Accept(string value = "")
            => _fieldUser.ModifyQuests(q => q.Accept(_questTemplateID, value)).Wait();

        public void Update(string value)
            => _fieldUser.ModifyQuests(q => q.Update(_questTemplateID, value)).Wait();

        public void Update(string key, string value)
            => _fieldUser.ModifyQuests(q => q.Update(_questTemplateID, key, value)).Wait();

        public void UpdateEx(string value)
            => _fieldUser.ModifyQuests(q => q.UpdateEx(_questTemplateID, value)).Wait();

        public void UpdateEx(string key, string value)
            => _fieldUser.ModifyQuests(q => q.UpdateEx(_questTemplateID, key, value)).Wait();

        public void Complete()
            => _fieldUser.ModifyQuests(q => q.Complete(_questTemplateID)).Wait();

        public void Resign()
            => _fieldUser.ModifyQuests(q => q.Resign(_questTemplateID)).Wait();
    }
}