using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public record ItemBundleTemplate : ItemTemplate, IItemBundleTemplate
{

    public ItemBundleTemplate(int id, IDataProperty info) : base(id, info)
    {
        UnitPrice = info.Resolve<double>("unitPrice") ?? 0.0;

        ReqLevel = info.Resolve<int>("reqLevel") ?? 0;
        IncPAD = info.Resolve<int>("incPAD") ?? 0;
        
        MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
    }
    public double UnitPrice { get; }
    
    public int ReqLevel { get; }
    public int IncPAD { get; }
    
    public short MaxPerSlot { get; }
}
