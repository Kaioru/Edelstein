using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Shop.Commodities;

namespace Edelstein.Common.Gameplay.Shop.Commodities;

public class ModifiedCommodityManager : 
    Repository<int, IModifiedCommodity>, 
    IModifiedCommodityManager;
