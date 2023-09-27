using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify;

public class ModifyInventoryGroupContext : AbstractModifyInventory, IModifyInventoryGroupContext
{
    private readonly Dictionary<ItemInventoryType, ModifyInventoryContext> _contexts;

    public ModifyInventoryGroupContext(
        ICharacterInventories inventories,
        ITemplateManager<IItemTemplate> manager
    ) : this(
        new Dictionary<ItemInventoryType, IItemInventory>
        {
            { ItemInventoryType.Equip, inventories.Equip },
            { ItemInventoryType.Consume, inventories.Consume },
            { ItemInventoryType.Install, inventories.Install },
            { ItemInventoryType.Etc, inventories.Etc },
            { ItemInventoryType.Cash, inventories.Cash }
        },
        manager)
    {
    }

    private ModifyInventoryGroupContext(
        IDictionary<ItemInventoryType, IItemInventory> inventories,
        ITemplateManager<IItemTemplate> manager
    ) =>
        _contexts = inventories.ToDictionary(
            kv => kv.Key,
            kv => new ModifyInventoryContext(kv.Key, kv.Value, manager)
        );

    public override IEnumerable<AbstractModifyInventoryOperation> Operations =>
        _contexts.Values.SelectMany(c => c.Operations);

    public IModifyInventoryContext? this[ItemInventoryType type] =>
        _contexts.GetValueOrDefault(type);

    public override short Add(IItemSlot item) =>
        this[GetTypeByID(item.ID)]?.Add(item) ?? -1;

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
    
    public override void Clear()
    {
        foreach (var context in _contexts.Values)
            context.Clear();
    }

    public override short Add(int templateID) =>
        this[GetTypeByID(templateID)]?.Add(templateID) ?? -1;

    public override short Add(int templateID, short count) =>
        this[GetTypeByID(templateID)]?.Add(templateID, count) ?? -1;

    public override short Add(IItemTemplate template) =>
        this[GetTypeByID(template.ID)]?.Add(template) ?? -1;

    public override short Add(IItemTemplate template, short count) =>
        this[GetTypeByID(template.ID)]?.Add(template, count) ?? -1;

    public bool HasEquipped(BodyPart part) =>
        this[ItemInventoryType.Equip]?[(short)-(short)part] != null;

    public bool HasEquipped(int templateID) =>
        this[ItemInventoryType.Equip]?.Items
            .Where(kv => kv.Key < 0)
            .Count(kv => kv.Value.ID == templateID) > 0;

    public bool HasEquipped(IItemTemplate template) =>
        HasEquipped(template.ID);

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
