using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public class ModifyInventoryGroupContext : AbstractModifyInventory, IModifyInventoryGroupContext
{
    private readonly Dictionary<ItemInventoryType, ModifyInventoryContext> _contexts;

    public ModifyInventoryGroupContext(
        IDictionary<ItemInventoryType,
            IItemInventory> inventories,
        ITemplateManager<IItemTemplate> _manager
    ) =>
        _contexts = inventories.ToDictionary(
            kv => kv.Key,
            kv => new ModifyInventoryContext(kv.Key, kv.Value, _manager)
        );

    public override IEnumerable<AbstractModifyInventoryOperation> Operations =>
        _contexts.Values.SelectMany(c => c.Operations);

    public IModifyInventoryContext? this[ItemInventoryType type] =>
        _contexts.GetValueOrDefault(type);

    public override void Add(IItemSlot item) =>
        this[GetTypeByID(item.ID)]?.Add(item);

    public override void Remove(int templateID) =>
        this[GetTypeByID(templateID)]?.Remove(templateID);

    public override void Remove(int templateID, short count) =>
        this[GetTypeByID(templateID)]?.Remove(templateID, count);

    public override void Remove(IItemTemplate template) =>
        this[GetTypeByID(template.ID)]?.Remove(template);

    public override void Remove(IItemTemplate template, short count) =>
        this[GetTypeByID(template.ID)]?.Remove(template, count);

    public override void RemoveAll(int templateID) =>
        this[GetTypeByID(templateID)]?.RemoveAll(templateID);

    public override void RemoveAll(IItemTemplate template) =>
        this[GetTypeByID(template.ID)]?.RemoveAll(template);

    public override void Gather()
    {
        foreach (var context in _contexts.Values)
            context.Gather();
    }

    public override void Sort()
    {
        foreach (var context in _contexts.Values)
            context.Sort();
    }

    public override void Add(int templateID) =>
        this[GetTypeByID(templateID)]?.Add(templateID);

    public override void Add(int templateID, short count) =>
        this[GetTypeByID(templateID)]?.Add(templateID, count);

    public override void Add(IItemTemplate template) =>
        this[GetTypeByID(template.ID)]?.Add(template);

    public override void Add(IItemTemplate template, short count) =>
        this[GetTypeByID(template.ID)]?.Add(template, count);

    public void SetEquipped(BodyPart part, int templateID) =>
        this[ItemInventoryType.Equip]?.SetSlot((short)-(short)part, templateID);

    public void SetEquipped(BodyPart part, int templateID, short count) =>
        this[ItemInventoryType.Equip]?.SetSlot((short)-(short)part, templateID, count);

    public void SetEquipped(BodyPart part, IItemTemplate template) =>
        this[ItemInventoryType.Equip]?.SetSlot((short)-(short)part, template.ID);

    public void SetEquipped(BodyPart part, IItemTemplate template, short count) =>
        this[ItemInventoryType.Equip]?.SetSlot((short)-(short)part, template.ID, count);

    private ItemInventoryType GetTypeByID(int id)
        => (ItemInventoryType)(id / 1_000_000);
}
