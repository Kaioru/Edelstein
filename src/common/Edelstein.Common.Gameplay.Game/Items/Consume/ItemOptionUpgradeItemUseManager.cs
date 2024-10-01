using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;
using Edelstein.Protocol.Gameplay.Game.Items.Options;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public class ItemOptionUpgradeItemUseManager(
    ITemplateManager<IItemTemplate> templates,
    IItemOptionsCalculator calculator
) : AbstractItemUseManager<IItemOptionUpgradeItemUseManagerContext, UserItemOptionUpgradeItemUseRequest, IItemBundleTemplate>(templates),
    IItemOptionUpgradeItemUseManager
{
    protected override IItemOptionUpgradeItemUseManagerContext Create(
        IFieldUser user,
        ItemSlotBase item,
        IItemBundleTemplate template,
        UserItemOptionUpgradeItemUseRequest info
    ) => new ItemOptionUpgradeItemUseManagerContext(user, item, template, info);

    protected override async Task<bool> Check(IItemOptionUpgradeItemUseManagerContext context, IFieldUser user)
    {
        if (user.Character.Inventories[ItemInventoryType.Equip]?[context.Info.EPOS] is not ItemSlotEquip) 
            return false;
        return await base.Check(context, user);
    }
    
    protected override async Task Handle(IItemOptionUpgradeItemUseManagerContext context, IFieldUser user)
    {
        var random = new Random();
        var success = random.NextDouble() < context.SuccessRate;
        var cursed = !success && random.NextDouble() < context.CursedRate;

        if (user.Character.Inventories[ItemInventoryType.Equip]?[context.Info.EPOS] is not ItemSlotEquip equip) 
            return;

        if (!context.SkipSuccess && success)
        {
            var options = await calculator.Calculate(equip);

            equip.Grade = (byte)options.Grade;
            equip.Option1 = (short)options.Option1;
            equip.Option2 = (short)options.Option2;
            equip.Option3 = (short)options.Option3;
            
            await user.ModifyInventory(i => i[ItemInventoryType.Equip]?.UpdateSlot(context.Info.EPOS));
        }

        if (!context.SkipCursed && cursed) 
            await user.ModifyInventory(i => i[ItemInventoryType.Equip]?.RemoveSlot(context.Info.EPOS));

        await user.Dispatch(new UserItemOptionUpgradeEffect
        {
            ObjectID = user.ObjectID ?? 0,
            Success = success,
            Cursed = cursed,
            EnchantSkill = context.Info.EnchantSkill
        });
    }
}
