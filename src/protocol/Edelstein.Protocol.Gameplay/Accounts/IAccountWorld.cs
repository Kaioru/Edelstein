using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Protocol.Gameplay.Accounts;

public interface IAccountWorld
{
    public IItemLocker Locker { get; set; }
    public IItemTrunk Trunk { get; set; }

    int CharacterSlotMax { get; set; }
}
