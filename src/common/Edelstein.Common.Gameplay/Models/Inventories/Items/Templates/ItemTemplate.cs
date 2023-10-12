using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates;

public record ItemTemplate : IItemTemplate
{
    public ItemTemplate(int id, IDataNode info)
    {
        ID = id;

        SellPrice = info.ResolveInt("price") ?? 0;
        TimeLimited = info.ResolveBool("timeLimited") ?? false;

        // TODO: replace

        Quest = info.ResolveBool("quest") ?? false;
        PartyQuest = info.ResolveBool("pquest") ?? false;
        Only = info.ResolveBool("only") ?? false;
        TradeBlock = info.ResolveBool("tradeBlock") ?? false;
        NotSale = info.ResolveBool("notSale") ?? false;
        BigSize = info.ResolveBool("bigSize") ?? false;
        ExpireOnLogout = info.ResolveBool("expireOnLogout") ?? false;
        AccountSharable = info.ResolveBool("accountSharable") ?? false;

        Cash = info.ResolveBool("cash") ?? false;
    }
    
    public int ID { get; }

    public int SellPrice { get; }
    public bool TimeLimited { get; }

    public int ReplaceTemplateID { get; }
    public int ReplaceMsg { get; }
    public int ReplacePeriod { get; }

    public bool Quest { get; }
    public bool PartyQuest { get; }
    public bool Only { get; }
    public bool TradeBlock { get; }
    public bool NotSale { get; }
    public bool BigSize { get; }
    public bool ExpireOnLogout { get; }
    public bool AccountSharable { get; }

    public bool Cash { get; }
}
