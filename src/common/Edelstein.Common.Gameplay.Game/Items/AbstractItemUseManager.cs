using System.Threading.Tasks;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items;

public abstract class AbstractItemUseManager<TContext, TTemplate>(
    ITemplateManager<IItemTemplate> templates
) : Pipeline<TContext>,
    IItemUseManager<TContext, TTemplate>
    where TContext : IItemUse<TTemplate>
    where TTemplate : class, IItemTemplate
{
    public async Task<TContext?> Process(int templateID)
    {
        if (await templates.Retrieve(templateID) is not TTemplate template)
            return default;

        var context = Create(template);

        await Process(context);
        return context;
    }

    public async Task Use(IFieldUser user, ItemInventoryType type, short slot, bool exclRequest = false)
    {
        var inventory = user.Character.Inventories[type];
        
        if (inventory == null)
        {
            await user.ModifyInventory(exclRequest: exclRequest);
            return;
        }

        inventory.Items.TryGetValue(slot, out var item);

        if (item == null)
        {
            await user.ModifyInventory(exclRequest: exclRequest);
            return;
        }
        
        var context = await Process(item.TemplateID);

        if (context == null)
        {
            await user.ModifyInventory(exclRequest: exclRequest);
            return;
        }

        await user.Modify(m =>
        {
            m.Inventory(
                i =>
                {
                    if (!context.SkipConsumption)
                        i[type]?.TakeSlot(slot);
                },
                exclRequest);
        });

        if (!context.SkipHandle)
            Handle(context, user);
    }

    protected abstract TContext Create(TTemplate template);
    protected abstract void Handle(TContext context, IFieldUser user);
}
