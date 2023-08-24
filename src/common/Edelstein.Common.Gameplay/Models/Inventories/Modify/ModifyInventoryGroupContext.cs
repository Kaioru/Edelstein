using System.Collections.Immutable;
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

    public override bool HasItem(int templateID) =>
        this[GetTypeByID(templateID)]?.HasItem(templateID) ?? false;
    public override bool HasItem(int templateID, short count) =>
        this[GetTypeByID(templateID)]?.HasItem(templateID, count) ?? false;

    public override bool HasItem(IItemTemplate template) =>
        this[GetTypeByID(template.ID)]?.HasItem(template) ?? false;
    public override bool HasItem(IItemTemplate template, short count) =>
        this[GetTypeByID(template.ID)]?.HasItem(template, count) ?? false;
    
    public override bool HasSlotFor(int templateID) => 
        this[GetTypeByID(templateID)]?.HasSlotFor(templateID) ?? false;
    public override bool HasSlotFor(int templateID, short count) =>
        this[GetTypeByID(templateID)]?.HasSlotFor(templateID, count) ?? false;
    public override bool HasSlotFor(ICollection<Tuple<int, short>> templates) =>
        templates
            .GroupBy(t => GetTypeByID(t.Item1))
            .All(g => this[g.Key]?.HasSlotFor(g.ToImmutableList()) ?? false);
    
    public override bool HasSlotFor(IItemTemplate template) =>
        this[GetTypeByID(template.ID)]?.HasSlotFor(template) ?? false;
    
    public override bool HasSlotFor(IItemTemplate template, short count) =>
        this[GetTypeByID(template.ID)]?.HasSlotFor(template, count) ?? false;
    
    public override bool HasSlotFor(ICollection<Tuple<IItemTemplate, short>> templates) =>
        templates
            .GroupBy(t => GetTypeByID(t.Item1.ID))
            .All(g => this[g.Key]?.HasSlotFor(g.ToImmutableList()) ?? false);
    
    public override bool HasSlotFor(IItemSlot item) =>
        this[GetTypeByID(item.ID)]?.HasSlotFor(item) ?? false;
    
    public override bool HasSlotFor(ICollection<IItemSlot> items) => 
        items
            .GroupBy(t => GetTypeByID(t.ID))
            .All(g => this[g.Key]?.HasSlotFor(g.ToImmutableList()) ?? false);


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
