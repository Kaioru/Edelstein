using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Items.Special;

namespace Edelstein.Common.Gameplay.Game.Items.Special;

public record StatChangeItemUse(
    IItemStatChangeTemplate Template
) : AbstractItemUse<IItemStatChangeTemplate>(Template), IStatChangeItemUse
{
    public int? HP { get; set; } = Template.HP;
    public int? MP { get; set; } = Template.MP;
}
