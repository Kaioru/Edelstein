using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Cash;
using Edelstein.Protocol.Gameplay.Game.Items.Options;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Cash;

public class ItemUnreleaseCashItemUseManager(
    ITemplateManager<IItemTemplate> templates,
    IItemOptionsCalculator calculator
) : AbstractCashItemUseManager<IItemUnreleaseCashItemUseManagerContext, StructuredItemUnreleaseCashItemUseInfoEx, IItemTemplate>(templates), 
    IItemUnreleaseCashItemUseManager
{
    protected override IItemUnreleaseCashItemUseManagerContext Create(
        IFieldUser user,
        ItemSlotBase item, 
        IItemTemplate template,
        ICashItemUseInfo info, 
        StructuredItemUnreleaseCashItemUseInfoEx infoEx
    )
        => new ItemUnreleaseCashItemUseManagerContext(user, item, template, info, infoEx);
    
    protected override async Task<bool> Check(IItemUnreleaseCashItemUseManagerContext context, IFieldUser user)
    {
        if (user.Character.Inventories[ItemInventoryType.Equip]?[(short)context.InfoEx.EPOS] is not ItemSlotEquip equip) 
            return false;
        if ((equip.Grade & (int)ItemOptionGradeState.Released) == 0)
            return false;
        return await base.Check(context, user);
    }
    
    protected override async Task Handle(IItemUnreleaseCashItemUseManagerContext context, IFieldUser user)
    {
        if (user.Character.Inventories[ItemInventoryType.Equip]?[(short)context.InfoEx.EPOS] is not ItemSlotEquip equip)
            return;

        var options = await calculator.Calculate(equip);

        equip.Grade = (byte)options.Grade;
        equip.Option1 = (short)options.Option1;
        equip.Option2 = (short)options.Option2;
        equip.Option3 = (short)options.Option3;
        
        await user.ModifyInventory(i => i[ItemInventoryType.Equip]?.UpdateSlot((short)context.InfoEx.EPOS));
        await user.Dispatch(new UserItemUnreleaseEffect
        {
            ObjectID = user.ObjectID ?? 0,
            Success = true
        });
    }
}
