using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Game.Objects.Drop;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Drop;

public class FieldDropItem : AbstractFieldDrop
{
    private readonly IItemSlot _item;

    public FieldDropItem(
        IPoint2D position,
        IItemSlot item,
        DropOwnType ownType = DropOwnType.NoOwn, 
        int ownerID = 0, 
        int sourceID = 0
    ) : base(position, ownType, ownerID, sourceID) 
        => _item = item;

    public override bool IsMoney => false;
    public override int Info => _item.ID;

    protected override Task<bool> Check(IFieldUser user) 
        => Task.FromResult(user.StageUser.Context.Managers.Inventory.HasSlotFor(user.Character.Inventories, _item));

    protected override async Task Update(IFieldUser user)
    {
        await user.ModifyInventory(i => i.Add(_item));
        await user.Message(new DropPickUpItemMessage(_item.ID, _item is IItemSlotBundle bundle ? bundle.Number : 1, false));
    }
}
