using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates;

public record ItemStringTemplate : IItemStringTemplate
{
    public int ID { get; }
    
    public string Name { get; }
    public string Desc { get; }
    
    public ItemStringTemplate(int id, IDataNode node)
    {
        ID = id;

        try
        {
            Name = node.ResolveString("name") ?? string.Empty;
            Desc = node.ResolveString("desc") ?? string.Empty;
        }
        catch (InvalidCastException)
        {
            // some item strings are wonky
            Name ??= string.Empty;
            Desc ??= string.Empty;
        }
    }
}
