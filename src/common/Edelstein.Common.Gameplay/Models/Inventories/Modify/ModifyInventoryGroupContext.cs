using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify;

public class ModifyInventoryGroupContext : AbstractModifyInventory, IModifyInventoryGroupContext
{
    private readonly Dictionary<ItemInventoryType, ModifyInventoryContext> _contexts;

    public override IEnumerable<AbstractModifyInventoryOperation> Operations =>
        _contexts.Values.SelectMany(c => c.Operations);

    public IModifyInventoryContext? this[ItemInventoryType type] =>
        _contexts.GetValueOrDefault(type);

    public ModifyInventoryGroupContext(
        IDictionary<ItemInventoryType,
        IItemInventory> inventories,
        ITemplateManager<IItemTemplate> manager
    ) =>
        _contexts = inventories.ToDictionary(
            kv => kv.Key,
            kv => new ModifyInventoryContext(kv.Key, kv.Value, manager)
        );

    public override bool Check(int templateID) =>
        this[GetTypeByID(templateID)]?.Check(templateID) ?? false;
    public override bool Check(int templateID, short count) =>
        this[GetTypeByID(templateID)]?.Check(templateID, count) ?? false;
    
    public override bool Check(IItemTemplate template) =>
        this[GetTypeByID(template.ID)]?.Check(template) ?? false;
    public override bool Check(IItemTemplate template, short count) =>
        this[GetTypeByID(template.ID)]?.Check(template, count) ?? false;

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

    public bool CheckEquipped(BodyPart part) =>
        this[ItemInventoryType.Equip]?[(short)-(short)part] != null;

    public bool CheckEquipped(int templateID) =>
        this[ItemInventoryType.Equip]?.Items
            .Where(kv => kv.Key < 0)
            .Count(kv => kv.Value.ID == templateID) > 0;

    public bool CheckEquipped(IItemTemplate template) =>
        CheckEquipped(template.ID);

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
