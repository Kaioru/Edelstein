using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Items.Options;

public interface IItemOptionsCalculatorContext
{
    ItemSlotEquip Equip { get; }
    IItemTemplate Template { get; }
    
    double GradeIncRateEpic { get; set; }
    double GradeIncRateUnique { get; set; }
    
    double Option2SetRate { get; set; }
    double Option2IncRate { get; set; }
    
    double Option3SetRate { get; set; }
    double Option3IncRate { get; set; }
}
