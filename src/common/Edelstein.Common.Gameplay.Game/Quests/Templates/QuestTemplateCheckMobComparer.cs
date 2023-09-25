using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public class QuestTemplateCheckMobComparer : IComparer<IQuestTemplateCheckMob>
{
    public int Compare(IQuestTemplateCheckMob? x, IQuestTemplateCheckMob? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var a = x.Order.CompareTo(y.Order);
        return a == 0 ? x.GetHashCode().CompareTo(y.GetHashCode()) : a;
    }
}

