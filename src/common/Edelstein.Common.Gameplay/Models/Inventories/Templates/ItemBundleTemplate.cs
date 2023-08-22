using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public record ItemBundleTemplate : ItemTemplate, IItemBundleTemplate
{
    public double UnitPrice { get; }
    public short MaxPerSlot { get; }
    
    public ItemBundleTemplate(int id, IDataProperty info) : base(id, info)
    {
        UnitPrice = info.Resolve<double>("unitPrice") ?? 0.0;
        MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
    }
}
