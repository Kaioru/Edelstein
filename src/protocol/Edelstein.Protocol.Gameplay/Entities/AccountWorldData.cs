using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Entities;

public class AccountWorldData : IRepositoryEntry<int>
{
    public int ID { get; set; }
    
    public int AccountID { get; set; }
    public int WorldID { get; set; }

    public ItemLocker Locker { get; set; } = new();
    public ItemTrunk Trunk { get; set; } = new();

    public int CharacterSlotMax { get; set; } = 3;
}
