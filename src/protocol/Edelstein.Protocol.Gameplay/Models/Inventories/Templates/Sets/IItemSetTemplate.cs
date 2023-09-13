using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;

public interface IItemSetTemplate : ITemplate
{
    int SetCompleteCount { get; }
    
    ICollection<int> Items { get; }
    IDictionary<int, IItemSetTemplateEffect> Effects { get; }
}
