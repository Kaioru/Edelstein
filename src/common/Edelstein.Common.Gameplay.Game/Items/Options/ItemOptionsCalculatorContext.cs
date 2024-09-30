using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items.Options;

namespace Edelstein.Common.Gameplay.Game.Items.Options;

public record ItemOptionsCalculatorContext(
    ItemSlotEquip Equip, 
    IItemTemplate Template
) : IItemOptionsCalculatorContext
{
    public double GradeIncRateEpic { get; set; }
    public double GradeIncRateUnique { get; set; }
    
    public double Option2SetRate { get; set; }
    public double Option2IncRate { get; set; }
    public double Option3SetRate { get; set; }
    public double Option3IncRate { get; set; }
}
