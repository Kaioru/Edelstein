using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates
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
