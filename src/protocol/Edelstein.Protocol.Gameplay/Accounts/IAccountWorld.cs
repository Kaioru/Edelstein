using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Accounts;

public interface IAccountWorld : IIdentifiable<int>
{
    public IItemLocker Locker { get; set; }
    public IItemTrunk Trunk { get; set; }

    int CharacterSlotMax { get; set; }
}
