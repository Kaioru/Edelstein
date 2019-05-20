using System.Threading.Tasks;
using Edelstein.Provider.Templates.Quest;

namespace Edelstein.Service.Game.Fields.Objects.User.Quests
{
    public static class QuestExtensions
    {
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