using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public record StatChangeItemUseManagerContext(
    ItemSlotBase Item,
    IItemStatChangeTemplate Template,
    UserStatChangeItemUseRequest Info
) : AbstractItemUseManagerContext<IItemStatChangeTemplate, UserStatChangeItemUseRequest>(Item, Template, Info), 
    IStatChangeItemUseManagerContext
{
    public int? HP { get; set; } = Template.HP;
    public int? MP { get; set; } = Template.MP;
    public int? HPr { get; set; } = Template.HPr;
    public int? MPr { get; set; } = Template.MPr;
}
