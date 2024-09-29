using System.Threading.Tasks;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract class AbstractCashItemUseManager<TContext, TInfoEx, TTemplate>(
    ITemplateManager<IItemTemplate> templates,
    bool initSkipConsumption = false
) : Pipeline<TContext>,
    ICashItemUseManager<TContext, TInfoEx, TTemplate>
    where TContext : ICashItemUseManagerContext<TTemplate, TInfoEx>
    where TInfoEx : ICashItemUseInfoEx
    where TTemplate : class, IItemTemplate
{
    public async Task Use(IFieldUser user, ItemInventoryType type, ICashItemUseInfo info, TInfoEx infoEx) 
    {
        try
        {
            var item = user.Character.Inventories[type]?.Items[info.Pos]!;
            var template = await templates.Retrieve(item.TemplateID);
            var context = Create(user, item, (template as TTemplate)!, info, infoEx);

            context.SkipConsumption = initSkipConsumption;
            
            if (!await Check(context, user))
            {
                await user.ModifyInventory(exclRequest: true);
                return;
            }
            
            await Process(context);
            
            await user.Modify(m =>
            {
                m.Inventory(
                    i =>
                    {
                        if (!context.SkipConsumption)
                            i[type]?.TakeSlot(info.Pos);
                    },
                    true);
            });

            if (!context.SkipHandle)
                await Handle(context, user);
        }
        catch
        {
            await user.ModifyInventory(exclRequest: true);
        }
    }
    
    protected abstract TContext Create(IFieldUser user, ItemSlotBase item, TTemplate template, ICashItemUseInfo info, TInfoEx infoEx);
    
    protected virtual Task<bool> Check(TContext context, IFieldUser user) => Task.FromResult(true);
    protected abstract Task Handle(TContext context, IFieldUser user);
}
