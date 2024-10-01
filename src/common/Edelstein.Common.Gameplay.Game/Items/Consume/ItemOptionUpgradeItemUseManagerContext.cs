using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public record ItemOptionUpgradeItemUseManagerContext(
    IFieldUser User,
    ItemSlotBase Item,
    IItemBundleTemplate Template,
    UserItemOptionUpgradeItemUseRequest Info
) : AbstractItemUseManagerContext<IItemBundleTemplate, UserItemOptionUpgradeItemUseRequest>(User, Item, Template, Info),
    IItemOptionUpgradeItemUseManagerContext
{
    public double Prob { get; set; } = (Template.ID % 10) switch
    {
        0 => 0.9, // Advanced Potential Scroll
        1 => 0.7, // Potential Scroll
        _ => 0.0
    };

    public bool SkipCursed { get; set; }
}
