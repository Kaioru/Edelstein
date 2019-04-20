namespace Edelstein.Provider.Templates.Item
{
    public class ItemTemplate : ITemplate
    {
        public int ID { get; set; }

        public int SellPrice { get; set; }
        public bool TimeLimited { get; set; }

        public int ReplaceTemplateID { get; set; }
        public int ReplaceMsg { get; set; }
        public int ReplacePeriod { get; set; }

        public bool Quest { get; set; }
        public bool PartyQuest { get; set; }
        public bool Only { get; set; }
        public bool TradeBlock { get; set; }
        public bool NotSale { get; set; }
        public bool BigSize { get; set; }
        public bool ExpireOnLogout { get; set; }
        public bool AccountSharable { get; set; }

        public bool Cash { get; set; }

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