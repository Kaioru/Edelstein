using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Database.Entities.Characters;
using Edelstein.Provider.Templates.Quest;

namespace Edelstein.Service.Game.Fields.Objects.User.Quests
{
    public static class QuestExtensions
    {
        public static QuestState GetQuestState(this Character c, short templateID)
        {
            return c.QuestComplete.ContainsKey(templateID)
                ? QuestState.Complete
                : c.QuestRecord.ContainsKey(templateID)
                    ? QuestState.Perform
                    : QuestState.None;
        }

        public static string GetQuestRecord(this Character c, short templateID)
        {
            return c.QuestRecord.ContainsKey(templateID)
                ? c.QuestRecord[templateID]
                : string.Empty;
        }

        public static string GetQuestRecordEX(this Character c, short templateID)
        {
            return c.QuestRecordEx.ContainsKey(templateID)
                ? c.QuestRecordEx[templateID]
                : string.Empty;
        }

        public static IDictionary<string, string> GetQuestRecordDict(this Character c, short templateID)
        {
            return c.GetQuestRecord(templateID)
                .Split(';')
                .Select(v => v.Split('='))
                .ToDictionary(pair => pair[0], pair => pair[1]);
        }
        
        public static IDictionary<string, string> GetQuestRecordEXDict(this Character c, short templateID)
        {
            return c.GetQuestRecordEX(templateID)
                .Split(';')
                .Select(v => v.Split('='))
                .ToDictionary(pair => pair[0], pair => pair[1]);
        }
        
        public static string GetQuestRecord(this Character c, short templateID, string key)
        {
            var dictionary = c.GetQuestRecordDict(templateID);
            return dictionary.ContainsKey(key) ? dictionary[key] : string.Empty;
        }

        public static string GetQuestRecordEX(this Character c, short templateID, string key)
        {
            var dictionary = c.GetQuestRecordEXDict(templateID);
            return dictionary.ContainsKey(key) ? dictionary[key] : string.Empty;
        }

        public static async Task<QuestResult> Check(this QuestTemplate template, QuestState state, FieldUser user)
        {
            var check = template.Check[state];
            return QuestResult.ActSuccess;
        }

        public static async Task<QuestResult> Act(this QuestTemplate template, QuestState state, FieldUser user)
        {
            var act = template.Act[state];
            return QuestResult.ActSuccess;
        }
    }
}