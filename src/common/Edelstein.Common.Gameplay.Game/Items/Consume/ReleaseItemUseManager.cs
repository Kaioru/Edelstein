using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public class ReleaseItemUseManager(
    ITemplateManager<IItemTemplate> templates
) : AbstractItemUseManager<IReleaseItemUseManagerContext, UserItemReleaseRequest, IItemBundleTemplate>(templates),
    IReleaseItemUseManager
{
    protected override IReleaseItemUseManagerContext Create(
        IFieldUser user,
        ItemSlotBase item,
        IItemBundleTemplate template,
        UserItemReleaseRequest info
    ) => new ReleaseItemUseManagerContext(user, item, template, info);

    protected override async Task<bool> Check(IReleaseItemUseManagerContext context, IFieldUser user)
    {
        if (!context.Template.ID.IsReleaseItem())
            return false;
        if (user.Character.Inventories[ItemInventoryType.Equip]?[context.Info.EPOS] is not ItemSlotEquip equip)
            return false;
        if ((equip.Grade & (int)ItemOptionGradeState.Released) > 0)
            return false;
        if (await templates.Retrieve(equip.TemplateID) is not IItemEquipTemplate template)
            return false;
        if ((context.Template.ID % 10) switch
            {
                0 => 30,
                1 => 70,
                2 => 120,
                3 => 200,
                _ => -1
            } < template.ReqLevel)
            return false;
        return await base.Check(context, user);
    }

    protected override async Task Handle(IReleaseItemUseManagerContext context, IFieldUser user)
    {
        if (user.Character.Inventories[ItemInventoryType.Equip]?[context.Info.EPOS] is not ItemSlotEquip equip)
            return;

        equip.Grade = (byte)(equip.Grade | (int)ItemOptionGradeState.Released);

        await user.ModifyInventory(i => i[ItemInventoryType.Equip]?.UpdateSlot(context.Info.EPOS));
        await user.Dispatch(new UserItemReleaseEffect
        {
            ObjectID = user.ObjectID ?? 0,
            Pos = context.Info.EPOS
        });
    }
}
