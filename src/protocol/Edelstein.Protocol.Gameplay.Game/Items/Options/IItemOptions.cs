using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;

namespace Edelstein.Protocol.Gameplay.Game.Items.Options;

public interface IItemOptions
{
    ItemOptionGrade Grade { get; }
    
    int Option1 { get; }
    int Option2 { get; }
    int Option3 { get; }
}
