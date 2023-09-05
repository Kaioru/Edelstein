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

    protected override async Task<bool> Check(IFieldUser user)
    {
        var check = false;
        await user.ModifyInventory(i => check = i.HasSlotFor(_item));
        return check;
    }
    
    protected override Task Update(IFieldUser user) 
        => user.ModifyInventory(i => i.Add(_item));
}
