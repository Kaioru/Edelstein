using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates;

public record ItemBundleTemplate : ItemTemplate, IItemBundleTemplate
{

    public ItemBundleTemplate(int id, IDataNode info) : base(id, info)
    {
        UnitPrice = info.ResolveDouble("unitPrice") ?? 0.0;

        ReqLevel = info.ResolveInt("reqLevel") ?? 0;
        IncPAD = info.ResolveInt("incPAD") ?? 0;
        
        MaxPerSlot = info.ResolveShort("slotMax") ?? 100;
    }
    public double UnitPrice { get; }
    
    public int ReqLevel { get; }
    public int IncPAD { get; }
    
    public short MaxPerSlot { get; }
}
