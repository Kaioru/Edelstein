using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public record ReleaseItemUseManagerContext(
    IFieldUser User,
    ItemSlotBase Item,
    IItemBundleTemplate Template,
    UserItemReleaseRequest Info
) : AbstractItemUseManagerContext<IItemBundleTemplate, UserItemReleaseRequest>(User, Item, Template, Info),
    IReleaseItemUseManagerContext;
