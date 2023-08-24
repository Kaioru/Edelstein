using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public record ItemStringTemplate : IItemStringTemplate
{
    public int ID { get; }
    
    public string Name { get; }
    public string Desc { get; }
    
    public ItemStringTemplate(int id, IDataProperty property)
    {
        ID = id;

        try
        {
            Name = property.ResolveOrDefault<string>("name") ?? string.Empty;
            Desc = property.ResolveOrDefault<string>("desc") ?? string.Empty;
        }
        catch (InvalidCastException)
        {
            // some item strings are wonky
            Name ??= string.Empty;
            Desc ??= string.Empty;
        }
    }
}
