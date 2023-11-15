using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Objects.User.Effects;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers.Facades;

public class SpeakerUserInventory : ISpeakerUserInventory
{
    private readonly IFieldUser _user;
    
    public SpeakerUserInventory(IFieldUser user) => _user = user;

    public void Add(int templateID, short count = 1)
    {
        _user.ModifyInventory(i => i.Add(templateID, count)).Wait();
        _user.Effect(new QuestEffect(new List<Tuple<int, int>> { Tuple.Create(templateID, (int)count) })).Wait();
    }

    public void Remove(int templateID, short count = 1)
    {
        _user.ModifyInventory(i => i.Remove(templateID, count)).Wait();
        _user.Effect(new QuestEffect(new List<Tuple<int, int>> { Tuple.Create(templateID, -count) })).Wait();
    }

    public int CountItem(int templateID)
        => _user.StageUser.Context.Managers.Inventory.CountItem(_user.Character.Inventories, templateID);
    
    public bool HasItem(int templateID, short count = 1)
        => _user.StageUser.Context.Managers.Inventory.HasItem(_user.Character.Inventories, templateID, count);
    
    public bool HasEquipped(int templateID)
        => _user.StageUser.Context.Managers.Inventory.HasEquipped(_user.Character.Inventories, templateID);
    
    public bool HasSlotFor(int templateID, short count = 1)
        => _user.StageUser.Context.Managers.Inventory.HasSlotFor(_user.Character.Inventories, templateID, count);
    
    public bool HasSlotFor(IDictionary<int, short> templates) 
        => _user.StageUser.Context.Managers.Inventory.HasSlotFor(_user.Character.Inventories, templates
            .Select(kv => Tuple.Create(kv.Key, kv.Value))
            .ToImmutableArray());
}
