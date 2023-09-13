using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;

public interface IItemOptionTemplate : ITemplate
{
    ItemOptionGrade Grade { get; }
    ItemOptionType Type { get; }
    
    short ReqLevel { get; }
    
    IDictionary<int, IItemOptionTemplateLevel> Levels { get; }
}
