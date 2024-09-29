using System.Threading.Tasks;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract class AbstractCashItemUseManager<TContext, TInfo, TInfoEx, TTemplate>(
    ITemplateManager<IItemTemplate> templates
) : Pipeline<TContext>,
    ICashItemUseManager<TContext, TInfo, TInfoEx, TTemplate>
    where TContext : ICashItemUseManagerContext<TTemplate, TInfo, TInfoEx>
    where TInfo : ICashItemUseInfo
    where TInfoEx : ICashItemUseInfoEx
    where TTemplate : class, IItemTemplate
{
    public async Task Use(IFieldUser user, ItemInventoryType type, TInfo info, TInfoEx infoEx, bool exclRequest = false) 
    {
        try
        {
            var item = user.Character.Inventories[type]?.Items[info.Pos]!;
            var template = await templates.Retrieve(item.TemplateID);
            var context = Create(item, (template as TTemplate)!, info, infoEx);

            await Process(context);
            
            await user.Modify(m =>
            {
                m.Inventory(
                    i =>
                    {
                        if (!context.SkipConsumption)
                            i[type]?.TakeSlot(info.Pos);
                    },
                    exclRequest);
            });

            if (!context.SkipHandle)
                Handle(context, user);
        }
        catch
        {
            await user.ModifyInventory(exclRequest: exclRequest);
        }
    }
    
    protected abstract TContext Create(ItemSlotBase item, TTemplate template, TInfo info, TInfoEx infoEx);
    protected abstract void Handle(TContext context, IFieldUser user);
}
