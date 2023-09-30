using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public class NPCShopTemplateItemComparer : IComparer<INPCShopItem>
{
    public int Compare(INPCShopItem? x, INPCShopItem? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var a = x.Order.CompareTo(y.Order);
        return a == 0 ? x.GetHashCode().CompareTo(y.GetHashCode()) : a;
    }
}

