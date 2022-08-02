using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Inventories.Templates;

public record ItemBundleTemplate : ItemTemplate, IItemBundleTemplate
{
    public ItemBundleTemplate(int id, IDataProperty info) : base(id, info)
    {
        UnitPrice = info.Resolve<double>("unitPrice") ?? 0.0;
        MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
    }

    public double UnitPrice { get; }
    public short MaxPerSlot { get; }
}
