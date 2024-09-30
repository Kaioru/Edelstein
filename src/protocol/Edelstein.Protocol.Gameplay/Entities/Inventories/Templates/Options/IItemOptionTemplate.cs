using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;

public interface IItemOptionTemplate : ITemplate
{
    ItemOptionGrade Grade { get; }
    ItemOptionType Type { get; }
    
    short ReqLevel { get; }
    
    ITemplateCollection<IItemOptionTemplateLevel> Levels { get; }
}
