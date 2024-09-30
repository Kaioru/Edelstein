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
    
    protected override async Task Handle(IItemOptionUpgradeItemUseManagerContext context, IFieldUser user)
    {
        var random = new Random();
        var success = random.Next() < context.Prob;
        const bool cursed = true;
        var equip = user.Character.Inventories[ItemInventoryType.Equip]?.Items[context.Info.EPOS]! as ItemSlotEquip;

        if (equip == null) return;
        
        await user.Message((await calculator.Calculate(equip)).ToString() ?? "");
        await user.Dispatch(new UserItemOptionUpgradeEffect
        {
            ObjectID = user.ObjectID ?? 0,
            Success = success,
            Cursed = cursed
        });
    }
}
