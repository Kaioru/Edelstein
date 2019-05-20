using System;
using System.Collections.Generic;
using Edelstein.Database.Entities.Characters;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Quests;

namespace Edelstein.Service.Game.Fields.Objects.User.Quests
{
    public class ModifyQuestContext
    {
        private readonly Character _character;
        public Queue<AbstractQuestMessage> Messages { get; }

        public ModifyQuestContext(Character character)
        {
            _character = character;
            Messages = new Queue<AbstractQuestMessage>();
        }

        public void Accept(short templateID, string value = "")
            => Update(templateID, value);

        public void Update(short templateID, string value)
        {
            _character.QuestRecord[templateID] = value;
            Messages.Enqueue(new PerformQuestRecordMessage(templateID, value));
        }

        public void UpdateEx(short templateID, string value)
        {
            _character.QuestRecordEx[templateID] = value;
            Messages.Enqueue(new QuestRecordExMessage(templateID, value));
        }

        public void Complete(short templateID, DateTime? dateComplete = null)
        {
            dateComplete ??= DateTime.Now;

            _character.QuestRecord.Remove(templateID);
            _character.QuestComplete[templateID] = (DateTime) dateComplete;
            Messages.Enqueue(new CompleteQuestRecordMessage(templateID, (DateTime) dateComplete));
        }

        public void Resign(short templateID)
        {
            if (_character.QuestComplete.ContainsKey(templateID))
            {
                _character.QuestComplete.Remove(templateID);
                Messages.Enqueue(new ResignQuestRecordMessage(templateID, true));
                return;
            }

            _character.QuestRecord.Remove(templateID);
            Messages.Enqueue(new ResignQuestRecordMessage(templateID, false));
        }
    }
}