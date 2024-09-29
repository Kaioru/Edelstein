using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public record StatChangeItemUseManagerContext(
    IItemStatChangeTemplate Template
) : AbstractItemUseManagerContext<IItemStatChangeTemplate>(Template), IStatChangeItemUseManagerContext
{
    public int? HP { get; set; } = Template.HP;
    public int? MP { get; set; } = Template.MP;
    public int? HPr { get; set; } = Template.HPr;
    public int? MPr { get; set; } = Template.MPr;
}
