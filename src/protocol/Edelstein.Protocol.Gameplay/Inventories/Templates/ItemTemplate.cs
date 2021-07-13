using Edelstein.Protocol.Gameplay.Templating;

namespace Edelstein.Protocol.Gameplay.Inventories.Templates
{
    public record ItemTemplate : ITemplate
    {
        public int ID { get; init; }

        public int SellPrice { get; init; }
        public bool TimeLimited { get; init; }

        public int ReplaceTemplateID { get; init; }
        public int ReplaceMsg { get; init; }
        public int ReplacePeriod { get; init; }

        public bool Quest { get; init; }
        public bool PartyQuest { get; init; }
        public bool Only { get; init; }
        public bool TradeBlock { get; init; }
        public bool NotSale { get; init; }
        public bool BigSize { get; init; }
        public bool ExpireOnLogout { get; init; }
        public bool AccountSharable { get; init; }

        public bool Cash { get; init; }
    }
}
