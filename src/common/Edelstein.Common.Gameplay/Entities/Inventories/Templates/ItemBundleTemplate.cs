using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Templates;

public record ItemBundleTemplate : ItemTemplate, IItemBundleTemplate
{
    public double UnitPrice { get; }
    
    public int ReqLevel { get; }
    public int IncPAD { get; }
    
    public short MaxPerSlot { get; }
    
    public ItemBundleTemplate(int id, IDataNode info) : base(id, info)
    {
        UnitPrice = info.ResolveDouble("unitPrice") ?? 0.0;

        ReqLevel = info.ResolveInt("reqLevel") ?? 0;
        IncPAD = info.ResolveInt("incPAD") ?? 0;
        
        MaxPerSlot = info.ResolveShort("slotMax") ?? 100;
    }
}
