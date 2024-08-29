using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Modifiers;

public class ModifyInventoryContextGroup(
    CharacterInventories inventories,
    ITemplateManager<IItemTemplate> templates
) : AbstractModifyInventory, IModifyInventoryContextGroup
{
    public override IEnumerable<StructuredModifyInventoryOperation> Operations => _contexts.Values
        .SelectMany(c => c.Operations)
        .ToList();
    
    private readonly ImmutableDictionary<ItemInventoryType, ModifyInventoryContext> _contexts
        = new Dictionary<ItemInventoryType, ItemInventory>
        {
            {
                ItemInventoryType.Equip, inventories.Equip
            },
            {
                ItemInventoryType.Consume, inventories.Consume
            },
            {
                ItemInventoryType.Install, inventories.Install
            },
            {
                ItemInventoryType.Etc, inventories.Etc
            },
            {
                ItemInventoryType.Cash, inventories.Cash
            }
        }.ToImmutableDictionary(
            kv => kv.Key,
            kv => new ModifyInventoryContext(kv.Key, kv.Value, templates)
        );
    
    public IModifyInventoryContext? this[ItemInventoryType type] =>
        _contexts.TryGetValue(type, out var context)
            ? context 
            : null;
    
    private static ItemInventoryType GetTypeByID(int id)
        => (ItemInventoryType)(id / 1_000_000);
    
    public override short Add(ItemSlotBase item) 
        => this[GetTypeByID(item.TemplateID)]?.Add(item) ?? -1;
    
    public override short Add(int templateID, short count = 1)
        => this[GetTypeByID(templateID)]?.Add(templateID, count) ?? -1;
    
    public override short Add(IItemTemplate template, short count = 1)
        => this[GetTypeByID(template.ID)]?.Add(template, count) ?? -1;
    
    public override void Remove(int templateID, short count = 1) 
        => this[GetTypeByID(templateID)]?.Remove(templateID, count);
    
    public override void RemoveAll(int templateID) 
        => this[GetTypeByID(templateID)]?.RemoveAll(templateID);
    
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

    public void SetEquipped(BodyPart part, int templateID, short count = 1) 
        => this[ItemInventoryType.Equip]?.SetSlot((short)-(short)part, templateID, count);
    
    public void SetEquipped(BodyPart part, IItemTemplate template, short count = 1) 
        => this[ItemInventoryType.Equip]?.SetSlot((short)-(short)part, template.ID, count);
}
