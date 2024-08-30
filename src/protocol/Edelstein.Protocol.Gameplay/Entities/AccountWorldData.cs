using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Entities;

public record AccountWorldData : IRepositoryEntry<int>
{
    public int ID { get; set; }
    
    public required int AccountID { get; set; }
    public required int WorldID { get; set; }

    public ItemLocker Locker { get; set; } = new();
    public ItemTrunk Trunk { get; set; } = new();

    public int CharacterSlotMax { get; set; } = 3;
}
