using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Item
{
    public class ItemTemplate : ITemplate
    {
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

        public ItemTemplate(int id, IDataProperty info)
        {
            ID = id;

            SellPrice = info.Resolve<int>("price") ?? 0;
            TimeLimited = info.Resolve<bool>("timeLimited") ?? false;

            // TODO: replace

            Quest = info.Resolve<bool>("quest") ?? false;
            PartyQuest = info.Resolve<bool>("pquest") ?? false;
            Only = info.Resolve<bool>("only") ?? false;
            TradeBlock = info.Resolve<bool>("tradeBlock") ?? false;
            NotSale = info.Resolve<bool>("notSale") ?? false;
            BigSize = info.Resolve<bool>("bigSize") ?? false;
            ExpireOnLogout = info.Resolve<bool>("expireOnLogout") ?? false;
            AccountSharable = info.Resolve<bool>("accountSharable") ?? false;

            Cash = info.Resolve<bool>("cash") ?? false;
        }
    }
}