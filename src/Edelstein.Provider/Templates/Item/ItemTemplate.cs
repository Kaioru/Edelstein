using Edelstein.Provider.Parser;

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
        
        public virtual void Parse(int id, IDataProperty p)
        {
            ID = id;

            SellPrice = p.Resolve<int>("info/price") ?? 0;
            TimeLimited = p.Resolve<bool>("info/timeLimited") ?? false;

            // TODO: replace

            Quest = p.Resolve<bool>("info/quest") ?? false;
            PartyQuest = p.Resolve<bool>("info/pquest") ?? false;
            Only = p.Resolve<bool>("info/only") ?? false;
            TradeBlock = p.Resolve<bool>("info/tradeBlock") ?? false;
            NotSale = p.Resolve<bool>("info/notSale") ?? false;
            BigSize = p.Resolve<bool>("info/bigSize") ?? false;
            ExpireOnLogout = p.Resolve<bool>("info/expireOnLogout") ?? false;
            AccountSharable = p.Resolve<bool>("info/accountSharable") ?? false;

            Cash = p.Resolve<bool>("info/cash") ?? false;
        }
    }
}