using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Extensions.Templates;
using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Quest;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Types;
using MoreLinq.Extensions;

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
                .Where(a => a.Length > 1)
                .ToDictionary(pair => pair[0], pair => pair[1]);
        }

        public static IDictionary<string, string> GetQuestRecordEXDict(this Character c, short templateID)
        {
            return c.GetQuestRecordEX(templateID)
                .Split(';')
                .Select(v => v.Split('='))
                .Where(a => a.Length > 1)
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

            if (act.EXP > 0)
            {
                await user.ModifyStats(s => s.EXP += act.EXP);
                await user.Message(new IncEXPMessage
                {
                    EXP = act.EXP,
                    OnQuest = true
                });
            }

            if (act.Items.Any())
            {
                await user.ModifyInventory(i =>
                    act.Items.ForEach(ii =>
                        i.Add(
                            user.Service.TemplateManager.Get<ItemTemplate>(ii.TemplateID),
                            (short) ii.Quantity
                        )
                    ));
            }

            return QuestResult.ActSuccess;
        }
    }
}