using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Consume;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Templates.Consume;

public record ItemStatChangeTemplate : ItemBundleTemplate, IItemStatChangeTemplate
{
    public int? HP { get; }
    public int? MP { get; }
    public int? HPr { get; }
    public int? MPr { get; }
    
    public ItemStatChangeTemplate(int id, IDataNode info, IDataNode? spec) : base(id, info)
    {
        HP = spec?.ResolveInt("hp");
        MP = spec?.ResolveInt("mp");
        HPr = spec?.ResolveInt("hpR");
        MPr = spec?.ResolveInt("mpR");
    }
}
