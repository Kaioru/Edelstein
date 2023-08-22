using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

public interface IItemTemplate : ITemplate
{
    int SellPrice { get; }
    bool TimeLimited { get; }

    int ReplaceTemplateID { get; }
    int ReplaceMsg { get; }
    int ReplacePeriod { get; }

    bool Quest { get; }
    bool PartyQuest { get; }
    bool Only { get; }
    bool TradeBlock { get; }
    bool NotSale { get; }
    bool BigSize { get; }
    bool ExpireOnLogout { get; }
    bool AccountSharable { get; }

    bool Cash { get; }
}
